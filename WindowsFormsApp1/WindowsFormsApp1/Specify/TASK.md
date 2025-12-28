# 實作任務清單 (Tasks) - CC-Link 控制器

## 🟦 階段一：底層通訊定義 (Infrastructure)
> 目標：完成與 MDFUNC32/64.DLL 的直接對接，建立安全的 P/Invoke 介面與常數。

- [ ] Task 1.1: 建立 `MelsecApi` 靜態類別（序）
  - `[DllImport]` 匯入：`mdOpen`, `mdClose`, `mdDevRead`, `mdDevWrite`, `mdSend`, `mdReceive`, `mdSetTimeout`, `mdGetStatus`, `mdGetLastError`。
  - 設定 `CallingConvention`, `CharSet` 與 x86/x64 DLL 名稱（依驅動）。
  - 驗收：能成功載入 DLL（避免 `EntryPointNotFound`/`BadImageFormat`）。

- [ ] Task 1.2: 定義 MELSEC 常數與列舉（平行）
  - `DeviceCode`：`DEV_LB(23)`, `DEV_LW(24)`, `DEV_LX(21)`, `DEV_LY(22)`（以手冊核對）。
  - `ReturnCode` 映射表（常見 0x4000 系列）。
  - 驗收：常數與枚舉在示範方法中可被引用並通過編譯。

- [ ] Task 1.3: 實作 `MelsecException`（平行）
  - 統一例外：`ReturnCode`, `Category(Transient/Permanent/Configuration/Hardware)`, `Advice`, `ApiName`。
  - 工具方法：`FromCode(int code, string api)` 用於映射。
  - 驗收：拋出例外含完整欄位，Log 可讀。

- [ ] Task 1.4: 建立 `IMelsecApiAdapter` 與 `MelsecApiAdapter`（序）
  - 以介面包裝 P/Invoke，集中管理 DLL 呼叫與錯誤檢查。
  - 驗收：可用假資料/模擬呼叫通過基本流程。

## 🟩 階段二：核心功能開發 (Core Logic)
> 目標：提供非同步且執行緒安全的 C# API，符合憲章（職責分離、非同步、資源管理）。

- [ ] Task 2.1: `MelsecControlCard` 類別骨架（序）
  - 欄位：`_settings`, `_api`, `_pathHandle`, `_lock(SemaphoreSlim)`, `_status`, `_logger`。
  - 實作 `IDisposable`，保證 `mdClose` 執行。
  - 驗收：建立與銷毀流程不拋例外，`_lock` 正確釋放。

- [ ] Task 2.2: `OpenAsync` 與 `CloseAsync`（序）
  - `OpenAsync`：`mdOpen` → `mdSetTimeout` → 更新 `ControllerStatus`。
  - `CloseAsync`：`mdClose` → 清理 Handle/狀態。
  - 驗收：開閉 100 次循環無資源洩漏（Handle 不增長）。

- [ ] Task 2.3: 讀寫 API 封裝（序）
  - `ReadWordsAsync`(LW)、`ReadBitsAsync`(LB/LX/LY)：解析位址 → `mdDevRead` → 轉型。
  - `WriteWordsAsync`、`WriteBitsAsync`：解析位址 → `mdDevWrite`。
  - 支援 `CancellationToken` 與逾時、重試退避（3 次/50ms）。
  - 驗收：在 Mock/實機上正確回傳資料，UI 不凍結。

- [ ] Task 2.4: 分批 API（平行）
  - `ReadWordsPagedAsync/WriteWordsPagedAsync(pageSize)`，避免超時。
  - 驗收：大筆資料（≥1000 words）在分批策略下成功且延遲可接受。

- [ ] Task 2.5: 位址解析器（平行）
  - `ParseAddress/TryParseAddress`：支援 `(LB|LW|LX|LY)\d+` 與批量長度；邊界檢查。
  - 驗收：錯誤格式拋 `ArgumentException`，合法輸入解析正確。

## 🟨 階段三：資料處理與監控 (Utilities & Monitoring)
> 目標：簡化 PLC 數據操作與連線管理，提供狀態事件。

- [ ] Task 3.1: 資料轉換工具（平行）
  - `WordPairToInt32`, `WordPairToFloat`（處理大小端）。
  - 驗收：測試數據轉換正確（含邊界/極值）。

- [ ] Task 3.2: 連線狀態與心跳（序）
  - `GetStatusAsync` 包 `mdGetStatus`；`Timer` 週期心跳，觸發 `StatusChanged`。
  - 驗收：拔插網路/卡時能偵測狀態變更並記錄。

## 🟧 階段四：WinForm 介面整合 (UI Integration)
> 目標：在 Windows 10/11 上驗證功能與使用者體驗。

- [ ] Task 4.1: 設定與初始化（序）
  - 從 `app.config` 或 JSON 讀取 `ControllerSettings`，防禦性驗證（IP/Port/Station 等）。
  - 驗收：缺值/格式錯誤時提示並阻止開啟。

- [ ] Task 4.2: 診斷視窗（平行）
  - 顯示 `PathHandle`、狀態、最近錯誤、延遲統計。
  - 驗收：UI 執行緒安全更新（`BeginInvoke`），長任務顯示 `WaitCursor`。

- [ ] Task 4.3: 定時更新邏輯（序）
  - `System.Windows.Forms.Timer` + 非同步讀寫，顯示即時數值。
  - 驗收：連續更新 10 分鐘無凍結或記憶體異常。

## ✅ 驗收標準（Acceptance Checklist）
- 開閉通訊 100 次無資源洩漏；`ControllerStatus.IsConnected` 正確。
- `ReadWordsAsync("LW100",10)` 與 `WriteBitsAsync("LY0",...)` 在實機/模擬下正確。
- 大量傳輸（100 次 × 100 words）平均延遲 ≤ 50ms，95 百分位合理；UI 不凍結。
- 發生錯誤時能拋 `MelsecException` 並記錄 ReturnCode/ApiName/建議動作。
- 心跳能偵測斷線並回復或回報。

## ⚠️ 風險與回滾（Risks & Rollback）
- DLL 版本/位元數不匹配 → 明確平台目標（x86/x64），在 `app.config` 提示並阻止啟動。
- 競態導致通訊失敗 → 強制序列化 `_pathHandle` 的所有操作（`SemaphoreSlim`）。
- 超時與大量批次 → 使用分批 API 與退避策略，必要時降低 `pageSize`。
- 驅動未安裝或 COM 未註冊 → 啟動自檢並提供修復指引；回滾至 Mock 控制器模式。

## 📎 參考（Reference）
- API：`mdOpen`, `mdClose`, `mdSetTimeout`, `mdSend`, `mdReceive`, `mdDevRead`, `mdDevWrite`, `mdGetStatus`, `mdGetLastError`。
- 裝置：`DEV_LB(23)`, `DEV_LW(24)`, `DEV_LX(21)`, `DEV_LY(22)`（以手冊確認）。