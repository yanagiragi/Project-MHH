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

    > 斷尾XDDD

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

* Day 4:
    
    > AI 簡單構想

        {
            canAttack = checkAttackInterval() 
            
            // 延遲時序更新怪物的轉向?
            if(canAttack && !isAttacking){ 
                playerPos = GetPlayerPosition() 
                canAttack = false; 
                isAttacking = true 
            } 
            
            if(isAttacking){ 
                combo_int = getComboBaseOnRandom() 
                preformComoHIt() 
            }
            
            else{ 
                if(isWalking) // update track player when walking
                RotateRotationToPosition(playerPos)
            }
        }

    > Lock Dependicies:

        Sleep
        Idle2
        Idle3

    > 火龍在Glide時的高度：

        524.21

    > 暫時沒有閃光彈閃下來，因為找不太到火龍的高處倒地動畫 & 倒地後站起來動畫

    > 不過真的要應該是可以兜出來 先當成LOD1

    > 如果要做閃光彈效果，白色的UI + Curve fade out + motion blur 可以做到類似的效果

    > 目前 Landing 只有 空中 Idle -> 倒地

    > 設定：

        空中Glide時無敵

        衝撞時只有Hit=2有機會打斷 Hit=7無法(不過因為Hit2應該本來就很難打斷了?)

        在空中時只有小機會你有辦法攻擊到他 (例如Glide經過你，或是空中Hit)

        // 這邊可能要把collider設小一點讓玩家有機會, 或是讓玩家的劍像賽菲羅斯那樣長?

    > 火龍 Hit

        Hit = 0     Idle
        Hit = 1     正面被攻擊小後退
        Hit = 2     側躺倒地
        Hit = 3     頭部被攻擊大後退
        Hit = 4     Tackle到一半跌倒 // Attack Hit2, 80 frame 內要拉起來

    > 火龍 Attack

        -1-9 !0 都是地上的attack
        
        Hit = -1    就叫而已

        Hit = 0     Idle
        Hit = 1     咬咬
        Hit = 2     衝刺 + 咬 // 被攻擊會有失敗動畫
        Hit = 3     左腳踩
        Hit = 4     右腳踩 // Mirror 左腳
        Hit = 5     兩腳交替踩+咬
        Hit = 6     跳起來採
        Hit = 7     衝撞 + 跌倒
        Hit = 8     單發火球
        Hit = 9     三連火球

        Hit = 11    空中左腳攻擊
        Hit = 12    空中右腳攻擊
        Hit = 13    下咬

    <details>
    <summary>Bugs require Fix:</summary>

        LOD0:

        > 跑步拔劍動畫怪怪的 (關掉Layer2 好像還是一樣)

        > 跑步無法翻滾

        > 應該喝藥水那邊有foot IK沒有處理好

        > 盾的動畫

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

* Day 5

    > 用 SubStateMachine + UnityStandardAssets裡面的範例把Haku的Animator重購了一次

    > 第三階層武器 結束有點卡卡的

    > 砍玩馬上跑沒有Blending

    > 砍 <-> 移動的 錯位已經被調整到比較可以接受了, 但是還有改善空間

    > 用 startAttackFlag & canStartAttackFlag 暫時Disable 移動控制，不過在結尾會有很小的機會可以動做完直接轉 (不過跟走路的直接轉差不多了)

    > 跑步時後手部可以多一些動畫 (額外的State)

    > 考慮一下要不要disable run & 喝藥時Disable Run

    > DrawSword2 動畫 Hit 感覺有點怪 不過看起來要觸發有點難

    > 現在只會往後倒...不確定是不是個問題

    <details>
    <summary>Bugs require Fix:</summary>

        LOD0:

            暫無

        LOD1: 

        > 盾的動畫 // 非絕對必要

        > 走路動畫 位置等等可以Blend得更漂亮

        > 跑步會有點錯位

        > 攻擊完會有點錯位

        > 因為攻擊直接關掉Layer2部分會有錯位的感覺

        > Combo 4 先丟著 需要修bug

        > 第三階層武器 結束有點卡卡的

        > 砍玩馬上跑沒有Blending

        > 砍 <-> 移動的 錯位已經被調整到比較可以接受了, 但是還有改善空間

        > 用 startAttackFlag & canStartAttackFlag 暫時Disable 移動控制，不過在結尾會有很小的機會可以動做完直接轉 (不過跟走路的直接轉差不多了)

        > 跑步時後手部可以多一些動畫 (額外的State)

        > 要不要disable run & 喝藥時Disable Run

    </details>

* Day 6

    > 昨天讓 P 先生試玩的心得
            
        * 沒把滑鼠隱藏 (應該是把lock cursor勾起來就好了 )

        * 新操作方式比較好

        * 新增滾輪拉近拉遠

        * 滑鼠靈敏度不夠
        
        * 如果你想讓玩家看不到內褲，也許你可以把"攝影機地面" 移到腰部左右的高度。這是我見過最可惡的設計 (轉貼原文)

    > 現在的火龍翅膀部分的collider沒有綁得很漂亮 不過至少倒地時還可以

    > 巨劍開掛模式 要做!!!!!!!!!!!! 

    > DustLoRes : loading 畫面

    > 還缺：

        怪物AI

        血量控制

        UI

        音效

    > 跑步無法翻滾

    > 翻滾完街道Idle的Blending非常糟糕

    > 攻擊後的delay (AttackThreshold.y)

        第幾項     對應哪個攻擊      Thres
        0           -1              5
        1           1               6.5
        2           2               7
        3           3               4
        4           4               4
        5           5               5
        6           6               4
        7           7               12
        8           8               11
        9           9               13.5

    > Grounded Update Threshold (For Now)

    65  99  100

    > 有看過空間Atk時 ArrayOutofBound (不過應該是因為Unity的Bug沒有更新變數的關係)

    > 空中攻擊後的delay (AttackThreshold.y)

        第幾項     對應哪個攻擊      Thres
        0           11              3
        1           12              3
        2           13              5
        3           14(大吼)          5
    
    > 暫時先不處理 滑翔時的旋轉角度 火球

    > 有一次有看到 AI line 388 有 AugmentException 的問題

    > 倒地有時候起來會怪怪的

    > Combo 第三招好像有點怪怪的? 完全砍不到? >> 接巨劍搞定

    > 有時候會有攻擊HitBox沒有出現攻擊動畫? 而且不只是一開始時會而已 部過似乎不常見

    > 跑步時沒有喝水動畫

    > 被擊飛時動畫樹會有點怪 可能是因為 Layer 1 被拉起來了

    > Haku 表情微調? 先丟著吧
    
    > 傷害數字? 先暫時不加入
    