# SPECIFICATION — CC-Link IE 控制卡通訊封裝 (.NET Framework 4.8)

## 1. 目標與背景 (Goals & Context)
目標：開發一個 C# 基礎類別庫，用於封裝 Mitsubishi Q80BD-J71GP21S-SX 控制卡的通訊邏輯，提供穩定直覺的 API。

背景與動機：
- 目前 WinForms 與 PLC 的交互過於底層，需手動管理 Channel 與緩衝區，開發負擔高且易出錯。
- 希望以 Service/MVP 架構抽離業務邏輯，讓 UI 僅負責綁定與觸發，改善維護性與穩定性。
- 依憲法要求：非同步、資源管理、例外處理與測試皆需落地。

範圍：封裝 Q80BD 系列（含 J71GP21S-SX）之 Link Bit/Word、Link Input/Output 讀寫；不覆蓋上層業務規則與配方管理。

## 2. 核心功能需求 (Functional Requirements)
2.1 通訊生命週期管理
- 提供 `OpenAsync()` 與 `CloseAsync()` 管理通訊路徑、資源與背景監測。
- 自動檢查控制卡硬體狀態與網路連線；若未就緒或驅動未安裝，回傳具體原因（型號、驅動版本、可疑通道）。
- 支援重試與回復：在暫時性錯誤（網路抖動）時可設定重試次數與退避時間。

2.2 資料讀寫能力
- 支援 Link Bit (`LB`)、Link Word (`LW`) 批量讀寫：如 `ReadWordsAsync("LW100", 10)`、`WriteWordsAsync("LW100", values)`。
- 支援 Link Input (`LX`) 與 Link Output (`LY`) 的存取：如 `ReadBitsAsync("LX0", 32)`、`WriteBitsAsync("LY0", bitArray)`。
- 提供位元（`bool`）與整數（`short`/`int`）的轉換 API，支援大小端配置（如需）。

2.3 非同步操作
- 所有通訊操作均為 `async/await`（Task-based），避免阻塞 UI 執行緒。
- 提供取消支援（`CancellationToken`），在大量批次時可安全中止。

2.4 狀態監控與診斷
- 提供 `GetStatusAsync()`：回覆控制卡狀態（連線、型號、通道、錯誤碼、最後錯誤、延遲統計）。
- 內建可選的心跳/連線檢查（可設定頻率），並提供事件/回呼給上層監控。

## 3. 架構設計要求 (Architectural Requirements)
3.1 抽象化通訊層
- 介面 `ICCLinkController`：定義開啟/關閉、讀寫（Bit/Word）、狀態取得等方法（皆為 `Task` 型別）。
- 具體實作 `MelsecIEControlCard`：封裝 Q80BD 系列的 MELSEC 通訊細節與錯誤碼對應。
- UI 層（`Form.cs`）僅使用 Controller/Service，禁止直接持有或呼叫底層通訊細節。

3.2 資料封裝
- 類別 `LinkDeviceAddress`：包含裝置種類（LB/LW/LX/LY）、基底地址（如 `LW0`）、偏移量、長度、位寬等。
- 類別 `ReadResult<T>`：包含資料、時間戳、來源、診斷（延遲、重試次數）、錯誤碼（如有）。

3.3 異常處理機制
- 統一錯誤處理：封裝 MELSEC 函式庫回傳的 Return Code → `CCLinkException`（含錯誤碼、簡述、建議動作）。
- 區分可重試與不可重試錯誤；提供策略（重試、斷線重連、回報）。
- 所有公共 API 需要 `try-catch` + 記錄 Log（NLog/Serilog），避免吞例外。

3.4 資源管理
- 嚴格遵守 `IDisposable` 用 `using`；控制卡/通訊物件在 `CloseAsync()` 正確釋放。
- 禁止在 UI 執行緒進行阻塞 IO；必要時用 `ConfigureAwait(false)`。

3.5 部署與相容性
- 明確 x86/x64 相容性與需註冊的 COM/DLL（若使用廠商提供元件）。
- 需要的權限（管理員/服務安裝）與安裝路徑（驅動與函式庫）。

## 4. 預期行為 (User Experience / Logic Flow)
- 初始化：建立 `MelsecIEControlCard` 並注入必要設定（IP/通道/站號等）。
- 開啟：呼叫 `OpenAsync()`；若控制卡未就緒或驅動未安裝，擲出 `CCLinkException`（含清楚訊息與 Return Code）。
- 讀取：`var words = await ReadWordsAsync("LW100", 10);` 取得 10 個暫存器值；UI 以 `BeginInvoke` 更新控制項。
- 寫入：`await WriteBitsAsync("LY0", bitArray);` 完成輸出控制；寫入結果記錄於 Log。
- 關閉：應用程式結束或視窗關閉時，呼叫 `CloseAsync()` 釋放資源；不可遺漏。
- 長任務期間：UI 顯示 `WaitCursor`、禁用相關按鈕、操作結束後恢復。

## 5. 非功能性需求 (Non-Functional Requirements)
- 性能：單次批量讀寫（100 words）在正常網路下平均延遲 ≤ 50 ms；可在規格外提供量測方法。
- 穩定性：暫時性錯誤自動重試（預設 3 次，退避 50ms）；不可重試錯誤立即回報。
- 安全與健壯性：所有輸入參數皆有防禦性檢查（null/範圍/格式）；禁止未處理例外。
- 記錄：使用 NLog/Serilog，最少記錄開啟/關閉、錯誤、重試、延遲統計。
- 相依：標註並隔離 MELSEC 相關 DLL/COM 的相依性；避免與 UI 直連。
- 兼容：.NET Framework 4.8、C# 7.3；WinForms UI 高 DPI 設定維持現狀（此庫不關注 UI 佈局）。
- 高負載行為：對大量批次操作採分批與退避策略（見 API 擴充）。

## 6. 組態 (Configuration)
- 連線參數：IP、Port/通道、Network/Station、超時、重試次數、退避時間、心跳間隔。
- 設定來源：`app.config` 或 JSON；提供 `ControllerSettings` 類型以便注入。
- 大小端/位元序：必要時可設定（預設按控制卡規格）。
- 逾時與取消：所有 API 支援 `CancellationToken`，並遵守 `TimeoutMs` 設定。

## 7. API 初稿 (Interfaces — 精簡版)
````````
public interface ICCLinkController
{
    Task OpenAsync(CancellationToken ct = default);
    Task CloseAsync(CancellationToken ct = default);

    Task<IReadOnlyList<bool>> ReadBitsAsync(string address, int count, CancellationToken ct = default);
    Task WriteBitsAsync(string address, IEnumerable<bool> values, CancellationToken ct = default);

    Task<IReadOnlyList<short>> ReadWordsAsync(string address, int count, CancellationToken ct = default);
    Task WriteWordsAsync(string address, IEnumerable<short> values, CancellationToken ct = default);

    Task<ControllerStatus> GetStatusAsync(CancellationToken ct = default);
}

public sealed class LinkDeviceAddress
{
    public string Kind { get; }   // LB, LW, LX, LY
    public int Base { get; }      // e.g., 0 for LW0
    public int Offset { get; }    // e.g., 100 for LW100
    public int Length { get; }    // e.g., 10 for LW100~LW109
    // 可加入位元序與驗證邏輯
}
````````

### 7.1 API 擴充（分批/事件/解析）
- `Task<IReadOnlyList<short>> ReadWordsPagedAsync(string address, int totalCount, int pageSize, CancellationToken ct = default)`
- `Task WriteWordsPagedAsync(string address, IEnumerable<short> values, int pageSize, CancellationToken ct = default)`
- `event EventHandler<ControllerStatusChangedEventArgs> StatusChanged`
- `ParseAddress(string)` 與 `TryParseAddress(string, out LinkDeviceAddress)`

## 8. 位址解析與驗證 (Address Parsing & Validation)
- 格式：`(LB|LW|LX|LY)\d+`，例如 `LW0`, `LB128`；批量操作以起始位址+長度表示。
- 範圍驗證：位址不得為負；批量長度 > 0；字串無效時擲出格式錯誤（`ArgumentException`）。
- 大小端：針對 `Word/DoubleWord` 提供位元序選項；預設遵循控制卡規格。

## 9. 錯誤碼映射與分類 (Return Code Mapping)
- 將 MELSEC Return Code 對應至 `CCLinkException`：
  - 類別：`Transient`（可重試）、`Permanent`（不可重試）、`Configuration`（設定/環境）、`Hardware`（硬體故障）。
- 例外訊息最低欄位：錯誤碼、簡述、建議動作、來源 API、時間戳。
- 重試策略：預設 3 次、退避 50ms，可於 `ControllerSettings` 覆寫。

## 10. 測試計畫 (Testing Plan)
- 單元測試：`LinkDeviceAddress` 解析與邊界、資料轉換（bits/words）。
- 整合測試：Open/Close、批量讀寫、錯誤碼映射、重試策略、逾時與取消。
- 模擬：`MockCCLinkController` 可設定固定延遲、注入錯誤碼、回傳資料模式（漸增/固定/隨機）。
- 量測方法：在正常網路環境下連續 100 次讀取 100 words，記錄平均/95 百分位延遲與錯誤率。

## 11. 接受準則 (Acceptance Criteria)
- 正常環境可成功 `OpenAsync()` 並回傳 `ControllerStatus`（含卡型號與連線就緒）。
- `ReadWordsAsync("LW100", 10)` 回傳 10 個 `short`，資料正確且延遲符合性能要求。
- 硬體未就緒或驅動缺失時，`OpenAsync()` 擲出 `CCLinkException`（含 Return Code 與人類可讀說明）。
- UI 不凍結：大量讀寫（≥100 words）期間 WinForms 可操作；控制項更新使用 `BeginInvoke`。
- `CloseAsync()` 後釋放資源、Log 出現關閉事件與統計，無殘留 Handle。

## 12. 環境矩陣 (Environment Matrix)
- 控制卡：Q80BD-J71GP21S-SX（列出已驗證韌體/驅動版本）。
- OS：Windows 10/11 x64（版本與 Build）。
- .NET：.NET Framework 4.8，C# 7.3。
- 相依：MELSEC/CC-Link 函式庫版本與安裝步驟（x86/x64、COM 註冊）。

## 13. 範例使用流程 (Sample Usage Flow)
1. 建立 `ControllerSettings` 並初始化 `MelsecIEControlCard`。
2. `await OpenAsync(ct)`；如失敗擲 `CCLinkException`。
3. `var values = await ReadWordsAsync("LW100", 10, ct);`；以 `BeginInvoke` 更新 UI。
4. `await WriteBitsAsync("LY0", bits, ct);`；記錄 Log。
5. 結束前 `await CloseAsync(ct)`；釋放資源。