﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    // 第0個必須為0 最後一個很小值
    [Tooltip("x 到 x + 1 的值代表要打出這個連段要在前一個動作的x秒後, x+1秒之前按")]
    public float[] intervalBetweenAttacks;
    
    [Header("(forward, backward, left, right)")]
    public Vector4 scaleFactor = Vector4.one;
    
    [Header("Control")]
    [SerializeField]
    private float inputH;
    [SerializeField]
    private float inputV;

    [SerializeField]
    private bool canWalk = true;
    [SerializeField]
    private bool isWalk = true;

    [SerializeField]
    private bool canRoll = true;
    [SerializeField]
    private bool isRoll = false;
    
    [Header("Attack")]
    [SerializeField]
    private bool canStartAttackFlag = true;
    [SerializeField]
    private bool startAttackFlag = false;
    [SerializeField]
    private int attackNum = 0;
    [SerializeField]
    private float innerTime;

    private float threshold = 0.05f;
    private float epsilon = 0.01f;
    private Animator animController;

    void Start () {
        innerTime = intervalBetweenAttacks[0];
        animController = GetComponent<Animator>();
    }

    delegate void callback();
    
    void Update () {


        #region Walk Part
        if (Input.GetKeyDown(KeyCode.Space) && canRoll && !isRoll)
        {
            // 必須要是Idle才能翻滾，取消動作的感覺用武器完多久回歸canStartAttack的方式控制
            if (attackNum == 0 && !startAttackFlag && canStartAttackFlag)
            { 
                canRoll = false;
                isRoll = true;
                animController.SetBool("Roll", isRoll);

                canWalk = false;
                isWalk = false;

                // -1f for end of frame
                StartCoroutine(DelayAdjustWeight("Roll", false, .3f, delegate { isRoll = false; }, .8f, delegate { canRoll = true; canWalk = true; }));
            }
        }

        //inputH = Mathf.Lerp(inputH, Input.GetAxis("Horizontal"), Time.deltaTime);
        //inputV = Mathf.Lerp(inputV, Input.GetAxis("Vertical"), Time.deltaTime);

        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");

        isWalk = ((Mathf.Abs(inputH) - epsilon) > threshold) || ((Mathf.Abs(inputV) - epsilon) > threshold);
        isWalk &= animController.GetInteger("Attack") == 0 && animController.GetLayerWeight(1) == 1 && canWalk;
        
        animController.SetBool("Walk", isWalk);
        animController.SetFloat("InputH", inputH);
        animController.SetFloat("InputV", inputV);

        // 收劍的動作要讓腳sync的上半身layer影響小一點
        if (isWalk)
        {
            float scaleH = (inputH > 0f) ? scaleFactor.w : scaleFactor.z;
            float scaleV = (inputV > 0f) ? scaleFactor.x : scaleFactor.y;
            transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward * scaleV * inputV, Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, transform.position + transform.right * scaleH * inputH, Time.deltaTime);
        }
        #endregion

        #region Attack Part
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
        if (
            !isRoll &&
            startAttackFlag && 
            (innerTime > intervalBetweenAttacks[attackNum + 1] || attackNum >= intervalBetweenAttacks.Length - 1)
        )
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

        #endregion

    }

    IEnumerator DelayAdjustWeight(string stateName, bool result, float delay, callback callbackFunc = null, float secondaryDelay = 0f, callback SecondcallbackFunc=null)
    {
        if(delay < 0f)
            yield return new WaitForEndOfFrame();
        else
            yield return new WaitForSeconds(delay);

        animController.SetBool(stateName, result);

        if (callbackFunc != null)
        {
            callbackFunc();
        }

        if (SecondcallbackFunc != null)
        {
            if (secondaryDelay < 0f)
                yield return new WaitForEndOfFrame();
            else
                yield return new WaitForSeconds(secondaryDelay);
            SecondcallbackFunc();
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
            // original value: 1.2f, 1.5f, 2f
            float prefixTime = 1.2f;
            yield return new WaitForSeconds(prefixTime);
            attackNum = 0;
            animController.SetInteger("Attack", attackNum); // Addition Adjust Attack to Sync Leg Layer

            yield return new WaitForSeconds(1.5f - prefixTime);
            canStartAttackFlag = true;

            //yield return new WaitForSeconds(1.6f - 1.5f);
            if(animController.GetLayerWeight(1) != 1f && attackNum == 0)
                animController.SetLayerWeight(1, 1);
        }
        else if (atk == 2)
        {
            // original value: .2f, .4f
            yield return new WaitForSeconds(.2f);
            canStartAttackFlag = true;
            yield return new WaitForSeconds(.3f - .2f);
            if (animController.GetLayerWeight(1) != 1f && attackNum == 0)
                animController.SetLayerWeight(1, 1);
        }
        else if (atk == 1)
        {
            // original value: .2f, .6f
            yield return new WaitForSeconds(.2f);
            canStartAttackFlag = true;
            yield return new WaitForSeconds(.4f - .2f);
            if (animController.GetLayerWeight(1) != 1f && attackNum == 0)
                animController.SetLayerWeight(1, 1);
        }

        
        
    }

    // Code Need Refactor
    IEnumerator delayRoutine()
    {
        animController.SetBool("Roll", true);
        yield return new WaitForEndOfFrame();
        animController.SetBool("Roll", false);
    }
}
