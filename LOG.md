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

* 補血

    > 需要背包系統?

### 實作LOD

應該還是要 打擊 > 畫面

* LOD0:

    * 簡單過場

    * 怪物不要太黏人

    * 打擊連技 + 簡單的劍氣效果就好

    * 美術風格大致統一就好

    * 可能先平行光就好

    * 有補血罐 * 3

* LOD1:

    * 第三接連段可以做集氣 或是弄出劍氣之類的

    * QTE

    * 怪物要聰明點 噴火

    * 更好的劍氣效果 + 鏡頭控制

    * 怪物 collider 細部調整

    * 累積打擊值之後用換貼圖的方式製造受傷效果
    
    * 取消攻擊

    * 加入其他道具

    * 採集點補充道具

    * 加入場景細節, 簡單 GI

* LOD2: 

    * 怪物的美術加強 像是噴血 區域累積值 部位破壞

    * PBR 貼圖

    * 多一關? 在水平面打王

    * 多一把武器

    * 調整GI

### 實作需求

* 人物

* 怪物

* combo + 控制

* 打擊系統

* 血量 + UI

* 怪物 AI

* 場景

* 過場

* 音樂

### 實作規劃

* Day1 3/13

    > 蒐集所有素材

    > 尋找動畫

    > 接起來的動畫

    > 所有系統的prototype

* Day2 3/14

    > Combo 系統實裝 + 效果

* Day3 3/15

    > 怪物AI

* Day4 3/16

    > 整合前面的 + 大致雛形

    > 補血系統

* Day5 3/17

    > 場景

* Day6, Day7 

    > 彈性

# LOG

* Day 1:

    > 用 mixamo + humanoid 比起 mixamo + generic 損失不少細節 (後續：使用腳部的IK就好多了)

    > MMD4Mecanim 應該轉出的fbx要跟模型在同一層 否則貼圖會抓不到

    > TDA Office Haku 的問題是因為腿太長了 不符合比例 所以套動畫動起來很怪 (後續：不是主要原因)

    > 看起來透過直接修改動畫的骨頭名字成2b.fbx裡面想map到的骨頭名稱比起 讓Unity抓 humanoid自己map來的可控性多一點，不過實驗的結果是即便如此微調成適合2B的動畫還是要花上不少功夫

    > 用 mixamo rig 玩的角色基本上問題比較少

    > 先用MMD4Mecanim 轉成fbx(這樣才能保留UV)，在進max刪掉骨頭，再丟上去mixamo autorig，再套他的動畫

    > 如果直接用pmxeditor 轉成 obj，會有材質丟失的問題(沒經過mixamoUV會保留，不過材質應該還要再調整)

    > 如果直接跟mtl 跟 貼圖包在一起上傳mixamo，會抓不到的樣子

    > 如果沒有刪掉骨頭，MMD4Mecanim的骨頭對mixamo 的 mapping 沒那麼好，可能直接失敗or抓錯(TDA Office Haku的骨頭型態就是因為太特別所以下半身抓不到)

        90.!joint_koshikyanserumigi > 100.joint_RightHipD > 101.joint_RightKneeD > 102.joint_RightFootD > 103.joint_migiashisakiEX

    > 因為實在不知道怎麼處理mixamo裡面root motion的問題，所以最後的解決方式如下；

        還是讓他用mecanim控制器 + Apply Root Motion, 不過記得要到Animator裡面每個State裡面點開 Foot IK, 不然腳會炸掉

    > 動畫會往下掉 Orz, 貌似可以去import animation那邊直接條y offset

    > 下半身用inplace動畫 + 程式控制position 可行 只是可能沒有那麼漂亮

    > Idle 左 右 後 走路動畫

        MingHan Bai：如果使用Animator Component，且勾選Apply Root Motion。　請到該動畫的FBX檔案中的Animation頁籤，Root Transform Position Y 請勾選或取消勾選Bake，因為角色動畫的Root有上下位移，如果使用Apply Root Motion會因此影響角色的Y軸。　如果還是有異狀，嘗試調整Based Upon 為 Feet。　https://docs.unity3d.com/Manual/RootMotion.html

    
    <details>
    <summary>Bugs require Fix:</summary>

        > Idle 左 右 後 走路動畫

        > 下半身用inplace動畫 + 程式控制position 可行 只是可能沒有那麼漂亮

        >> 被攻擊的動畫

        > LOD1: Haku的動畫再漂亮一點點

    </details>

* Day 2:
    
    > Idle 左 右 後 走路動畫 好了，不過快速切換會有點跳格

    > 改為用滑鼠控制旋轉

    > 可能調整前不能左右翻滾 後面的

    > 不要多開Game, Input 可能會怪怪的

    > 不要讓他自動挑選windows Graphic API, MMD4Mecanim 會出不來

    > Combo 4 先丟著 需要修bug

    > 跑步動畫 只有前跑才有 後面走也有 但是只有1.1倍 有點難感覺到

    > 跑步拔劍

    > 跑步無法翻滾

    > 應該喝藥水那邊有foot IK沒有處理好

    > 喝水動畫 (不過腳部有一些小瑕疵)

    > 不知道為甚麼重新import MMD4Mecanim就可以抓到morph (MMD4Mecanim Model)了, 不過 Bullet Physics 又變糟糕了

    > 喝水動畫 好像再跑步的時候按會有blending沒有很漂亮的狀況

    > 受傷動畫 不過起來後rotation會有點變 此外起來時的動畫有點小位移 (應該是要調Blending)

    > Layer 2 Hit Blending 調整

    > 具體而言要怎麼實現 怪物打到你的碰撞 這點要想一下，用Kinematic 好像沒有辦法使用 Mass的性質

        也許真的只能用trigger, 只要隨時有撞到就算反向vector推開

        要判別碰到 & 被攻擊到的差異 

    <details>
    <summary>Bugs require Fix:</summary>

        LOD0:

        > 跑步拔劍動畫怪怪的 (關掉Layer2 好像還是一樣)

        > 跑步無法翻滾

        > 應該喝藥水那邊有foot IK沒有處理好

        > 盾的動畫

        > 怪物動畫    

        LOD1: 

        > Idle 走路動畫 快速切換會有點跳格

        > Combo 4 先丟著 需要修bug

        > 受傷動畫 不過起來後rotation會有點變 此外起來時的動畫有點小位移 (應該是要調Blending)

        > LOD1: Hit Blending 調整

        > LOD1: 喝水動畫 (不過腳部有一些小瑕疵)

        > LOD1: 走路動畫 -> 攻擊動畫 Fade會有點跳格

        > LOD1: Haku的動畫再漂亮一點點

    </details>


* Day 3:
    
    > 火龍的走路動畫還有有點小問題 不過至少看起來會像走了

    > root: ern??? > ern001_2 > NULL > COG, 要進Rig那邊手動選擇root motion

    > blending 真的可以調得很漂亮

    > 火龍在地面上 root 322.451
    
    > 火龍在空中   root 824.201

    > 火龍走路時目前沒有辦法直接切跑步 不過這個還好

    > 目前完成火龍動畫：

        直走 直衝 地上idle 空中idle 地上<->空中 空中大車輪 睡眠

    <details>
    <summary>Bugs require Fix:</summary>

        LOD0:

        > 跑步拔劍動畫怪怪的 (關掉Layer2 好像還是一樣)

        > 跑步無法翻滾

        > 應該喝藥水那邊有foot IK沒有處理好

        > 盾的動畫

        > 怪物動畫:

            地上攻擊 空中攻擊 受傷動畫 倒地動畫

        > 重構Haku的Animator

        LOD1: 

        > Idle 走路動畫 快速切換會有點跳格

        > Combo 4 先丟著 需要修bug

        > 受傷動畫 不過起來後rotation會有點變 此外起來時的動畫有點小位移 (應該是要調Blending)

        > LOD1: Hit Blending 調整

        > LOD1: 喝水動畫 (不過腳部有一些小瑕疵)

        > LOD1: 走路動畫 -> 攻擊動畫 Fade會有點跳格

        > LOD1: Haku的動畫再漂亮一點點

    </details>