# 實作計畫 (Implementation Plan) - CC-Link 控制類別庫

## 1. 技術背景與挑戰
* **目標硬體**：Mitsubishi Q80BD-J71GP21S-SX (CC-Link IE Control)
* **驅動程式**：MELSEC 通訊函式庫 (MDFUNC32.DLL / MDFUNC64.DLL)
* **開發環境**：.NET Framework 4.8, WinForm
* **核心挑戰**：
    * 處理 Unmanaged DLL 的 P/Invoke 呼叫。
    * 確保 WinForm 在大量點位讀寫時不卡頓。
    * 嚴格管理通訊路徑 (Path Handle) 的釋放。

## 2. 類別架構設計
我們將採用三層架構來確保代碼的可維護性：

1. **`MelsecApi.cs` (底層接口)**：
    * 靜態類別，封裝所有 `DllImport`。
    * 定義硬體常數（Device Codes, Return Codes）。
2. **`MelsecControlCard.cs` (核心邏輯)**：
    * 實作 `ICCLinkController` 介面。
    * 負責 `mdOpen` 與 `mdClose` 的生命週期管理。
    * 將同步的 API 封裝為 `Task`-based 非同步方法。
3. **`MelsecException.cs` (例外處理)**：
    * 將十六進制的錯誤代碼轉換為具體的錯誤描述訊息。

### 2.1 文字類別圖（Class Diagram）
- `ICCLinkController` (interface)
  - Methods: `Task OpenAsync(CancellationToken)`, `Task CloseAsync(CancellationToken)`, `Task<IReadOnlyList<short>> ReadWordsAsync(string,int,CancellationToken)`, `Task<IReadOnlyList<bool>> ReadBitsAsync(string,int,CancellationToken)`, `Task WriteWordsAsync(string,IEnumerable<short>,CancellationToken)`, `Task WriteBitsAsync(string,IEnumerable<bool>,CancellationToken)`, `Task<ControllerStatus> GetStatusAsync(CancellationToken)`
- `MelsecControlCard : ICCLinkController, IDisposable`
  - Fields: `_settings: ControllerSettings`, `_pathHandle: IntPtr`, `_api: IMelsecApiAdapter`, `_lock: SemaphoreSlim`, `_status: ControllerStatus`, `_logger`
  - Role: 非同步封裝、重試/逾時、例外映射、資源釋放與狀態事件
- `IMelsecApiAdapter`
  - Methods: `mdOpen`, `mdClose`, `mdSend`, `mdReceive`, `mdDevRead`, `mdDevWrite`, `mdGetStatus`, `mdGetLastError`, `mdSetTimeout`
- `MelsecApiAdapter : IMelsecApiAdapter`
  - 封裝 P/Invoke 呼叫至 `MDFUNC32.DLL` / `MDFUNC64.DLL`
- `MelsecApi` (static)
  - DllImport 定義與常數、結構
- `MelsecException : Exception`
  - Props: `ReturnCode`, `Category`, `Advice`, `ApiName`
- `ControllerSettings`
  - Props: `Ip`, `Port`, `Network`, `Station`, `TimeoutMs`, `RetryCount`, `RetryBackoffMs`, `HeartbeatIntervalMs`, `Endian`, `Isx64`
- `ControllerStatus`
  - Props: `IsConnected`, `Model`, `DriverVersion`, `Channel`, `LastErrorCode`, `AvgLatencyMs`, `MaxLatencyMs`, `LastUpdated`
- `LinkDeviceAddress`
  - Props: `Kind`(LB/LW/LX/LY), `Start`, `Length`, `BitWidth`, `Endian`

## 3. 關鍵 API 整合計畫
* **路徑管理**：
    * 使用 `mdOpen` 取得通路，並將其儲存於私有欄位 `_pathHandle`。
    * 使用 `mdClose` 於 `Dispose()` 方法中，確保資源回收。
    * 設定逾時：`mdSetTimeout(handle, TimeoutMs)`。
* **資料讀寫**：
    * 優先使用裝置級別 API：`mdDevRead(handle, deviceCode, start, count, short[] dest)`；`mdDevWrite(handle, deviceCode, start, count, short[] src)`。
    * 封包級別（必要時）：`mdSend(handle, byte[] buffer, int length)` / `mdReceive(handle, byte[] buffer, int length, out int received)`。
* **診斷**：
    * `mdGetStatus(handle, out MelsecStatus)` 取得卡/連線狀態。
    * `mdGetLastError(handle, out int code)` 取得最近錯誤碼。

### 3.1 MELSEC API 清單（需以手冊核對簽章）
- `int mdOpen(ref IntPtr handle, /* open params */)`
- `int mdClose(IntPtr handle)`
- `int mdSetTimeout(IntPtr handle, int timeoutMs)`
- `int mdSend(IntPtr handle, byte[] buffer, int length)`
- `int mdReceive(IntPtr handle, byte[] buffer, int length, out int received)`
- `int mdDevRead(IntPtr handle, int deviceCode, int startAddr, int count, short[] dest)`
- `int mdDevWrite(IntPtr handle, int deviceCode, int startAddr, int count, short[] src)`
- `int mdGetStatus(IntPtr handle, out MelsecStatus status)`
- `int mdGetLastError(IntPtr handle, out int code)`

### 3.2 裝置常數（示意，請依手冊確認）
- `DEV_LW = 24`（Link Word）
- `DEV_LB = 23`（Link Bit）
- `DEV_LX = <spec>`、`DEV_LY = <spec>`（J71GP21S-SX 規格表）

## 4. 實作任務清單 (Task List)

### Phase 1: 基礎基礎建設 (Infrastructure)
- [ ] 建立 `MelsecApi` 定義，加入 `mdOpen`, `mdClose`, `mdSend`, `mdReceive`、`mdSetTimeout`。
- [ ] 根據 Mitsubishi 手冊定義常用裝置常數 (e.g., `DEV_LW = 24`, `DEV_LB = 23`)。
- [ ] 實作 `MelsecException` 以處理 `0x4000` 系列錯誤碼，分類 `Transient/Permanent/Configuration/Hardware`。

### Phase 2: 核心功能封裝 (Core Logic)
- [ ] 實作 `OpenAsync` 與 `CloseAsync` 方法（更新 `ControllerStatus`）。
- [ ] 實作 `ReadWordsAsync` (LW) 與 `ReadBitsAsync` (LB/LX/LY)。
- [ ] 實作 `WriteWordsAsync`/`WriteBitsAsync`，支援批量與分批（paged）。
- [ ] 加入 `CancellationToken` 與逾時支援，重試退避策略（預設 3 次/50ms）。

### Phase 3: WinForm 範例與測試
- [ ] 建立簡單測試介面（Open/Read/Write/Close）與狀態顯示。
- [ ] 心跳：以 `Timer` 週期呼叫 `mdGetStatus`，狀態變更觸發 `StatusChanged` 事件。
- [ ] 量測：100 次 × 100 words 讀取，記錄平均/95 百分位延遲與錯誤率。

## 5. 注意事項
* **編譯目標**：必須確認驅動程式版本。若為 32 位元驅動，專案必須設定為 `x86`；若為 64 位元，則設為 `x64` (不可使用 Any CPU)。
* **P/Invoke**：明確 `DllImport` 的 `CallingConvention`、`CharSet`、x86/x64 DLL 名稱；建議以 `IMelsecApiAdapter` 包裝並集中管理。
* **資源與安全**：以 `SafeHandle`/`Dispose` 確保 `mdClose` 一定執行；所有 `IDisposable` 使用 `using`；避免在 UI 執行緒進行阻塞 IO。
* **多執行緒**：使用 `SemaphoreSlim` 串行化對同一路徑的讀寫，防止競態；提供 `CancellationToken`。 
* **批量策略**：大量資料採分批（`ReadWordsPagedAsync/WriteWordsPagedAsync`），降低逾時風險。
* **記錄**：NLog/Serilog 記錄 API 名稱、位址範圍、資料量、耗時、重試次數、ReturnCode；便於診斷。
* **環境與部署**：明確 x86/x64、COM/DLL 註冊、管理員權限與安裝路徑；在 `app.config` 或 JSON 指定設定來源。