using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    // sum of intervalBetweenAttacks < intervalBetweenWholeAttacks
    // 第0個必須為0 最後一個很小值
    public float[] intervalBetweenAttacks;
    
    public float intervalBetweenWholeAttacks;
    public int attackNum = 0;

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

        // TODO
        // check sum of intervalBetweenAttacks < intervalBetweenWholeAttacks
    }

    void Update () {

        if (startAttackFlag)
        {
            //outerTime -= Time.deltaTime;
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
                animController.SetLayerWeight(1, 0);
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

    IEnumerator restoreFlag(int atk)
    {
        Debug.Log(atk);
        if (atk == 3)
        {
            float prefixTime = 1.2f;
            yield return new WaitForSeconds(prefixTime);
            attackNum = 0;
            animController.SetInteger("Attack", attackNum);

            yield return new WaitForSeconds(1.5f - prefixTime);
            canStartAttackFlag = true;

            yield return new WaitForSeconds(2.0f - 1.5f);
            if(animController.GetLayerWeight(1) == 0 && attackNum == 0)
                animController.SetLayerWeight(1, 1);
        }
        else if (atk == 2)
        {
            yield return new WaitForSeconds(.2f);
            canStartAttackFlag = true;
            yield return new WaitForSeconds(.4f - .2f);
            if (animController.GetLayerWeight(1) == 0 && attackNum == 0)
                animController.SetLayerWeight(1, 1);
        }
        else if (atk == 1)
        {
            yield return new WaitForSeconds(.2f);
            canStartAttackFlag = true;
            yield return new WaitForSeconds(.6f - .2f);
            if (animController.GetLayerWeight(1) == 0 && attackNum == 0)
                animController.SetLayerWeight(1, 1);
        }

        
        
    }
}
