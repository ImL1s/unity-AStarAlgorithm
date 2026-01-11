# Unity A* 尋路演算法 (A* Pathfinding Algorithm)

這是一個在 Unity 引擎中實作 **A* (A-Star)** 尋路演算法的開源範例專案。此專案展示了如何在網格系統中計算從起點到終點的最短路徑，並透過 Gizmos 進行視覺化除錯。

## 🚀 專案功能 (Features)

- **核心尋路演算法**: 完整實作 A* 演算法邏輯，計算 `F = G + H` 代價。
- **動態網格系統 (Grid System)**: 自動生成可導航的網格地圖。
- **障礙物偵測**: 節點 (Node) 包含可行走與不可行走的屬性設定。
- **視覺化除錯 (Visualization)**: 使用 Unity Gizmos 在編輯器中繪製網格、路徑與節點狀態（`Open List`, `Closed List`）。

## 📂 檔案結構 (File Structure)

主要腳本位於 `Assets/Script/` 目錄下：

- **`FindPath.cs`**: 尋路系統的核心管理器，負責執行 A* 運算並回傳路徑。
- **`Grid.cs`**: 負責建立地圖網格，將世界座標轉換為網格座標，並管理所有節點。
- **`Node.cs`**: 定義單個節點的資料結構（包含世界座標、G/H/F 代價、父節點參照等）。
- **`LearnGizmos.cs`**: 用於測試與學習 Unity Gizmos 繪圖功能的輔助腳本。

## 🎮 如何使用 (Getting Started)

### 環境需求
- Unity Editor (建議使用 2019.4 LTS 或更新版本)

### 執行步驟
1. 下載或 Clone 此專案。
2. 使用 Unity Hub 開啟專案資料夾 (`unity-AStarAlgorithm/AStar`).
3. 在 Project 視窗中，導航至 `Assets/Scenes/`。
4. 雙擊開啟 **`AStar.unity`** 場景。
5. 點擊 Unity 編輯器上方的 **Play** ▶️ 按鈕。
6. 您應該能在 Scene 視窗或 Game 視窗中看到路徑生成的結果（需開啟 Gizmos 顯示）。

## 🛠️ 實作細節

A* 演算法透過以下公式評估每個節點：
$$F = G + H$$

- **G Cost**: 從起點移動到當前節點的距離代價。
- **H Cost (Heuristic)**: 從當前節點到終點的預估距離（通常使用曼哈頓距離或歐基里德距離）。
- **F Cost**: 總代價，路徑選擇時優先考慮 F 值最低的節點。

## 📝 License

此專案採用 MIT License。