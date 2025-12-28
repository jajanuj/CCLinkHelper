MPLC–Equipment Heartbeat
測試程式碼任務清單（Task Breakdown）
一、測試基礎架構（Test Infrastructure）
1. 建立測試專案骨架

 建立 Heartbeat 測試專案（Console / WinForm / NUnit / xUnit）

 定義測試共用常數

Check Timing

Timeout 次數 / 秒數

 定義模擬用狀態列舉

EquipmentPowerState

EquipmentOnlineState

HeartbeatResult

2. 建立通訊模擬層（Mock / Simulator）

 模擬 MPLC I/O 讀寫介面

Read LB

Write LB

 模擬設備端 I/O 行為

 支援手動 / 自動 Toggle Bit

二、Request Flag 測試（MPLC → Equipment）
3. Request Flag 產生測試

 測試 MPLC 是否依 Check Timing 週期切換 Request Flag

 驗證 Request Flag Toggle 行為是否正確

 驗證 Request Flag 不因設備狀態改變而停止送出

4. Request Flag 穩定性測試

 長時間測試（例如 1 小時）

 確認無 Bit 卡死 / 無異常重置

 確認 Timing 誤差在允許範圍內

三、Response Flag 測試（Equipment → MPLC）
5. 正常回應測試（Happy Path）

 設備 Power On + Online Mode

 偵測 Request Flag 變化

 Response Flag 正確 Toggle

 MPLC 判定設備為 Online

6. 設備未上線狀態測試

 Power Off 狀態

 Response Flag 不變

 MPLC 判定 Offline

 Power On 但未進入 Online Mode

 Response Flag 不變

 MPLC 判定 Offline

四、Online / Offline 判斷邏輯測試
7. Online 判定測試

 Response Flag 每次皆有變化

 MPLC 正確維持 Online 狀態

 不誤觸發 Alarm

8. Offline 判定測試

 Response Flag 固定不變

 累積達指定檢查次數

 MPLC 判定 Offline

 產生對應 Alarm

9. 間歇性異常測試（Flapping）

 Response Flag 偶爾變化

 驗證 MPLC 是否：

維持 Online

或依規則切換 Offline

 確認不產生誤報 Alarm

五、Timeout / Alarm 測試
10. Timeout 計時測試

 模擬 Response Flag 無回應

 驗證 Timer T1 啟動

 驗證 Timeout 時間是否正確

11. Alarm 內容驗證

 Alarm Code：Master PLC Interface Time Out

 Alarm 觸發時機正確

 Alarm 清除條件驗證（恢復心跳）

六、狀態切換測試
12. 設備狀態切換測試

 Offline → Online

 Online → Offline

 Power Off → Power On → Online

 驗證 MPLC 狀態轉換正確

13. 重連測試

 通訊中斷後恢復

 Response Flag 恢復 Toggle

 MPLC 正確回到 Online

 Alarm 正確解除

七、邊界與異常測試（Edge Case）
14. 非法操作測試

 設備主動送 Response Flag（未對應 Request）

 Request / Response 同時卡在 1

 Bit 抖動（高頻切換）

15. 通訊異常模擬

 CC-Link IE 中斷

 延遲回應

 封包遺失模擬（若有）

八、測試報告與工具化
16. Log 與 Trace

 Request / Response 時間戳記

 狀態變化紀錄

 Alarm 觸發紀錄

17. 測試報告輸出

 測試項目 Pass / Fail

 Offline 發生原因摘要

 可輸出 CSV / JSON