using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    // 第0個必須為0 最後一個很小值
    [Tooltip("x 到 x + 1 的值代表要打出這個連段要在前一個動作的x秒後, x+1秒之前按")]
    public float[] intervalBetweenAttacks;

    [SerializeField]
    private int attackNum = 0;
    [SerializeField]
    private bool canStartAttackFlag = true;
    [SerializeField]
    private bool startAttackFlag = false;
    [SerializeField]
    private float innerTime;
    
    private Animator animController;
    
    void Start () {
        innerTime = intervalBetweenAttacks[0];
        animController = GetComponent<Animator>();
    }

    void Update () {

        if (startAttackFlag)
        {
            innerTime += Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0) && canStartAttackFlag)
        {
            // 第一個連段
            if (!startAttackFlag && animController.GetBool("Draw")) // First Attack
            {
                startAttackFlag = true;
                ++attackNum;
                animController.SetInteger("Attack", attackNum);
                StartCoroutine(DelayAdjustWeight(1, 0));
            }
            else // 之後的連段
            {
                if (innerTime > intervalBetweenAttacks[attackNum])
                {
                    ++attackNum;
                    animController.SetInteger("Attack", attackNum);
                    innerTime = 0;
                }

            }
        }

        // 連到最後一招 or 给按的時間過了
        if (startAttackFlag && (innerTime > intervalBetweenAttacks[attackNum + 1] || attackNum >= intervalBetweenAttacks.Length - 1))
        {
            StartCoroutine(restoreFlag(attackNum));
            if (attackNum == 3)
            {
                ; // Sync Leg Layer Animation
            }
            else
            {
                attackNum = 0;
                animController.SetInteger("Attack", attackNum);
            }
            canStartAttackFlag = false;
            startAttackFlag = false;            
            innerTime = intervalBetweenAttacks[0]; // 0
        }
        
	}

    IEnumerator DelayAdjustWeight(int layer, float weight)
    {
        float delay = 0.1f;
        yield return new WaitForSeconds(delay);
        animController.SetLayerWeight(layer, weight);

        // Need More tweak on walk->Attake Fade

        // Below: weight drop too fast
        //float initWeight = animController.GetLayerWeight(layer);
        //float time = 0;
        //while((time += Time.deltaTime) < delay && animController.GetLayerWeight(layer) > weight)
        //{
        //    animController.SetLayerWeight(layer, (initWeight - weight) / delay * Time.deltaTime);
        //    yield return null;
        //}

    }

    IEnumerator restoreFlag(int atk)
    {
        Debug.Log(atk);
        if (atk == 3)
        {
            float prefixTime = 1.2f;
            yield return new WaitForSeconds(prefixTime);
            attackNum = 0;
            animController.SetInteger("Attack", attackNum); // Addition Adjust Attack to Sync Leg Layer

            yield return new WaitForSeconds(1.5f - prefixTime);
            canStartAttackFlag = true;

            yield return new WaitForSeconds(2.0f - 1.5f);
            if(animController.GetLayerWeight(1) != 1f && attackNum == 0)
                animController.SetLayerWeight(1, 1);
        }
        else if (atk == 2)
        {
            yield return new WaitForSeconds(.2f);
            canStartAttackFlag = true;
            yield return new WaitForSeconds(.4f - .2f);
            if (animController.GetLayerWeight(1) != 1f && attackNum == 0)
                animController.SetLayerWeight(1, 1);
        }
        else if (atk == 1)
        {
            yield return new WaitForSeconds(.2f);
            canStartAttackFlag = true;
            yield return new WaitForSeconds(.6f - .2f);
            if (animController.GetLayerWeight(1) != 1f && attackNum == 0)
                animController.SetLayerWeight(1, 1);
        }

        
        
    }
}
