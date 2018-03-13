# Dev Log

### 實作細節

* 打擊系統

    > Collision 判定會是個問題

    > 切到第一刀當flag ? 這樣的話用trigger ，看看要不要用程式控制打擊的回饋感

    > 彈刀要額外add force, 重點還是能不能有效用效果給人打擊感

    > 還要想辦法加入鏡頭

    > 不同地方的hitbox, 抗性, 累積值?

    > 連技

    > 用 Anim Speed 控制?

* 畫面

    > 一開始用就 Forward ?

    > 確保之前的shader可以用，但是就犧牲效能

    > MMD 的 Shader 到底有沒有兼容Deferred Shading

* 動畫

    > 可能需要在一天內把動畫弄上線

    > 怪物先用雌火龍 C4d -> fbx, root motion 那邊可能需要tweek 參數

    > 玩家的動畫可能通通得用mixamo

    > 武器可能會有點麻煩 要fit原本動畫的話

* 人物

    > 如果要用MMD + 良好的物理模擬，還是得盡量短髮+短裙比較好

    > shading 風格可能 要統一，看看要不要通通換stanard, 不過 MMD 那邊可能會有問題

    > 不知道改toon有沒有辦法

* 怪物

    > 簡單但是不要被看出來很簡單的AI機制

    > 受傷時blend動畫的控制

* 鏡頭

    > Cinemachine + 攝影學
