# MPLC – Equipment 心跳（Heartbeat）介面規格

## 1. 目的（Purpose）

本規格定義 **MPLC 與設備（Equipment）之間的心跳（Heartbeat）通訊機制**，用於確認雙方通訊是否正常、設備是否存活（Online / Offline），並作為上位系統（MES / LCS）監控設備狀態的依據。

---

## 2. 系統角色與定義（Roles & Definitions）

| 名稱 | 說明 |
|---|---|
| CPC | Central PC，上位系統，負責設定心跳與對時等系統策略與時機 |
| LCS | Line Control System，邏輯上的產線控制介面層 |
| MPLC | Master PLC，實際執行心跳控制與狀態判斷的 PLC |
| Equipment | 設備端（含 PC / 控制器），需回應心跳訊號 |

> 備註：
> - 本規格文件中所描述的 **LCS 行為，實際由 MPLC 執行**。
> - 設備端僅與 MPLC 通訊，不直接與 CPC 或 LCS 通訊。

---

## 3. 功能概要（Overview）

- MPLC 與設備需透過心跳訊號相互確認存活狀態。
- MPLC 需定期送出心跳請求（Request Flag）。
- 設備在符合條件時需回傳心跳回應（Response Flag）。
- MPLC 依據設備回傳之 Response Flag 變化情況，判斷設備 Online / Offline。
- MPLC 需將通訊狀態回報至上位系統（LCS / MES）。

---

## 4. 前置條件（Preconditions）

### 4.1 設備端狀態條件

設備僅在以下條件 **全部成立** 時，才可進行心跳回應：

- 設備 Power On
- 設備已進入 Online Mode（線上有效模式）

若設備在上述條件不成立時：
- 不回傳 Response Flag
- MPLC 應依規則判斷為 Offline 或異常

---

## 5. 通訊訊號定義（Signal Definition）

### 5.1 Request Flag（心跳請求）

| 項目 | 說明 |
|---|---|
| 名稱 | Request Flag / Send Clock |
| 方向 | MPLC → Equipment |
| 資料型態 | BIT |
| 位址 | LB **** |
| 說明 | MPLC 用於確認設備通訊狀態之心跳請求訊號 |

---

### 5.2 Response Flag（心跳回應）

| 項目 | 說明 |
|---|---|
| 名稱 | Response Flag / Response Clock |
| 方向 | Equipment → MPLC |
| 資料型態 | BIT |
| 位址 | LB **** |
| 說明 | 設備回應 MPLC 心跳請求之訊號 |

---

## 6. 心跳流程說明（Heartbeat Flow）

### 6.1 Request（MPLC → Equipment）

1. MPLC 依照設定的 **Check Timing（檢查週期）**，週期性改變（Toggle / Pulse）Request Flag。
2. MPLC 將 Request Flag 傳送至設備端。

---

### 6.2 Response（Equipment → MPLC）

1. 設備偵測 Request Flag 狀態變化。
2. 當設備處於 Power On 且 Online Mode 狀態時：
   - 設備需對應 Request Flag 的變化，改變（Toggle / Pulse）Response Flag。
3. 設備將 Response Flag 回傳至 MPLC。

---

### 6.3 狀態判斷（MPLC）

MPLC 需比較 **本次** 與 **前次** Response Flag 狀態：

- Response Flag 有變化：
  - 判定設備 Online
- Response Flag 在指定次數或時間內無變化：
  - 判定設備 Offline
  - 觸發異常處理

---

## 7. Offline / 異常判斷規則（Error Handling）

### 7.1 Offline 判定條件

設備符合以下任一條件時，MPLC 應判定設備 Offline：

- 在指定檢查次數內，Response Flag 無任何變化
- 設備未進入 Online Mode
- 通訊中斷（CC-Link IE 異常）

---

### 7.2 異常處理

- MPLC 需產生 Alarm
- Alarm 內容：
  - **Master PLC Interface Time Out**
- MPLC 需將異常狀態回報至上位系統（LCS / MES）

---

## 8. Timeout 定義（Timeout Definition）

| Timer | Alarm Text | 說明 | 設定值 |
|---|---|---|---|
| T1 | Master PLC Interface Time Out | 心跳通訊逾時異常 | 依專案設定 |

---

## 9. 注意事項（Notes）

- 心跳邏輯由 MPLC 統一管理，不因設備型式不同而調整。
- 設備端不得主動送出 Response Flag，僅能回應 Request Flag。
- 心跳通訊僅用於存活確認，不得承載其他業務邏輯。

---

## 10. 修訂紀錄（Revision History）

| 版本 | 日期 | 說明 |
|---|---|---|
| v1.0 | YYYY-MM-DD | 初版建立 |
