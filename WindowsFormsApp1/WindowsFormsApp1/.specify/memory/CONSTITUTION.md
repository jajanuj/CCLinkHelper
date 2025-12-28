# 專案憲法 (Project Constitution) - WinForm .NET 4.8

## 1. 核心原則 (Core Principles)
* **職責分離 (SoC)**：嚴禁將業務邏輯直接撰寫於 `Form.cs` 的事件處理器 (Event Handlers) 中。所有邏輯必須抽離至 Service、Logic 或 Presenter 層。
* **穩定性與資源管理**：確保所有 GDI+ 物件與資料庫連線在使用後皆正確釋放，預防記憶體洩漏。
* **非同步響應 (Async UI)**：任何超過 100ms 的操作（如 IO、API 或大型運算）必須使用 `async/await`，嚴禁阻塞 UI 主執行緒。

## 2. 技術棧 (Tech Stack)
* **Framework**: .NET Framework 4.8
* **Language**: C# 7.3+
* **UI**: Windows Forms (WinForms)
* **Dependency Management**: NuGet
* **Libraries**: 
    * 序列化: Newtonsoft.Json
    * 日誌: NLog 或 Serilog
    * 測試: NUnit 或 MSTest

## 3. 代碼風格與慣例 (Code Style)
* **命名規範 (Naming)**：
    * **控制項 (Controls)**：使用類型前綴，例如：`btnSave`, `lblStatus`, `txtUserName`, `dgvOrderList`。
    * **私有欄位**：使用下底線開頭的小駝峰，例如：`_userService`。
    * **類別與方法**：使用大駝峰 (PascalCase)。
* **架構要求**：
    * 必須實作簡單的 MVP (Model-View-Presenter) 或 Service-Based 架構。
    * 視窗間的資料傳遞應透過構造函數或事件，避免使用全域靜態變數。

## 4. UI/UX 規範 (UI/UX Standards)
* **佈局 (Layout)**：優先使用 `TableLayoutPanel` 與 `FlowLayoutPanel` 配合 `Dock` 與 `Anchor` 屬性，禁止寫死絕對座標。
* **High DPI**：必須在 `app.config` 或 `app.manifest` 中啟用 DPI 感知，確保在高解析度螢幕下不模糊。
* **使用者回饋**：執行長時任務時，必須將 `Cursor` 切換為 `WaitCursor` 並禁用相關按鈕。

## 5. 錯誤處理與健壯性 (Error Handling)
* **資源管理**：所有 `IDisposable` 對象（如 `SqlConnection`, `Stream`）必須使用 `using` 區塊。
* **例外攔截**：
    * 關鍵邏輯必須包含 `try-catch` 並記錄 Log。
    * UI 層級應實作全域異常捕捉 (`Application.ThreadException`)。
* **防禦性程式碼**：所有輸入參數與物件調用前必須進行 `null` 檢查。

## 6. 測試與維護 (Testing & Maintenance)
* **邏輯測試**：Service 與 Logic 層的方法應盡可能編寫單元測試。
* **文件化**：公用 (Public) 方法必須撰寫 `<summary>` 註解。