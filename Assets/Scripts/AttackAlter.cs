using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAlter : MonoBehaviour {

    // 第0個必須為0 最後一個很小值
    [Tooltip("x 到 x + 1 的值代表要打出這個連段要在前一個動作的x秒後, x+1秒之前按")]
    public Vector3[] intervalBetweenAttacks;
    
    [Header("(forward, backward, left, right)")]
    public Vector4 scaleFactor = Vector4.one;

    [Header("(forward, backward, left, right)")]
    public Vector4 rollScaleFactor = Vector4.one;

    public float runScale = 1.1f;

    [Header("Control")]
    [SerializeField]
    private float inputH;
    [SerializeField]
    private float inputV;

    //[SerializeField]
    public bool canWalk = true;
    [SerializeField]
    private bool isWalk = true;
    [SerializeField]
    private bool isRun = false;

    [SerializeField]
    private bool canRoll = true;
    [SerializeField]
    private bool isRoll = false;
    
    [Header("Attack")]
    [SerializeField]
    public bool canStartAttackFlag = true;
    //[SerializeField]
    public bool startAttackFlag = false;
    [SerializeField]
    private int attackNum = 0;
    [SerializeField]
    private float innerTime;

    [SerializeField]
    public bool isDrinking = false;

    [SerializeField]
    private bool isHit = false;

    private float threshold = 0.05f;
    private float epsilon = 0.01f;
    private Animator animController;

    void Start () {
        innerTime = 0;
        animController = GetComponent<Animator>();
    }

    delegate void callback();

    IEnumerator PreformHit()
    {
        yield return new WaitForSeconds(.2f);
        animController.SetBool("Hit", false);

        yield return new WaitForSeconds(3.8f);

        animController.SetLayerWeight(1, 1);

        isHit = false;
        canWalk = true;
        isWalk = false;
        isRun = false;
        canRoll = true;
        isRoll = false;
        canStartAttackFlag = true;
        startAttackFlag = false;
    }

    void Update () {

        // Temporary Flag
        if (Input.GetKeyDown(KeyCode.C) && !isHit)
        {
            isHit = true;
            animController.SetBool("Hit", true);
            StartCoroutine(PreformHit());
        }

        if (isHit)
        {
            canWalk = false;
            isWalk = false;
            isRun = false;
            //animController.SetBool("Run", isRun);
            isDrinking = false;
            canRoll = false;
            isRoll = false;
            canStartAttackFlag = false;
            startAttackFlag = false;
            attackNum = 0;
            inputH = 0;
            inputV = 0;
            animController.SetInteger("Attack", 0);
            animController.SetLayerWeight(1, 0);
            return;
        }

        ////float mouseX = Input.GetAxis("Mouse X");
        ////transform.Rotate(new Vector3(0, mouseX, 0));

        //float mouseX2 = Input.GetAxis("Horizontal");
        //transform.Rotate(new Vector3(0, mouseX2, 0));

        //#region Walk Part

        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");

        //isWalk = ((Mathf.Abs(inputH) - epsilon) > threshold) || ((Mathf.Abs(inputV) - epsilon) > threshold);
        //isWalk &= animController.GetInteger("Attack") == 0 && animController.GetLayerWeight(1) == 1 && canWalk;

        //animController.SetBool("Walk", isWalk);
        ////animController.SetFloat("InputH", inputH);
        //animController.SetFloat("InputV", inputV);

        //isRun = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Q) && canRoll && !isRoll && canStartAttackFlag && !startAttackFlag)
        {
            animController.SetBool("UsingItem", true);
            isDrinking = true;
            //StartCoroutine(DelayAdjustWeight(2, 0));
            //StartCoroutine(DelayAdjustWeight("UsingItem", false, -1, delegate { animController.SetLayerWeight(2, .46f); }, 1f));
            StartCoroutine(PreformDrink());
        }

        else if (Input.GetKeyDown(KeyCode.Space) && canRoll && !isRoll && (Mathf.Abs(inputV) + Mathf.Abs(inputH)) > Mathf.Epsilon)
        {
            // 必須要是Idle才能翻滾，取消動作的感覺用武器完多久回歸canStartAttack的方式控制
            if (attackNum == 0 && !startAttackFlag && canStartAttackFlag)
            {
                canRoll = false;
                isRoll = true;
                isRun = false;
                animController.SetBool("Roll", isRoll);

                canWalk = false;
                isWalk = false;

                // -1f for end of frame
                //StartCoroutine(DelayAdjustWeight("Roll", false, .3f, delegate { isRoll = false; }, .8f, delegate { canRoll = true; canWalk = true; }));
                StartCoroutine(PerformRoll());
            }
        }

        //animController.SetBool("Run", isRun);

        ////inputH = Mathf.Lerp(inputH, Input.GetAxis("Horizontal"), Time.deltaTime);
        ////inputV = Mathf.Lerp(inputV, Input.GetAxis("Vertical"), Time.deltaTime);


        //// 收劍的動作要讓腳sync的上半身layer影響小一點
        //if (isWalk && !isHit)
        //{
        //    float scaleH = (inputH > 0f) ? scaleFactor.w : scaleFactor.z;
        //    float scaleV = (inputV > 0f) ? scaleFactor.x : scaleFactor.y;

        //    if (isRun)
        //    {
        //        if (inputV < 0f)
        //            scaleV *= 1.1f;
        //        else
        //            scaleV *= runScale;

        //        Debug.Log("Run");

        //        //animController.speed = 2;
        //    }
        //    //else
        //    //{
        //    //    animController.speed = 1;
        //    //}

        //    transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward * scaleV * inputV, Time.deltaTime);
        //    //transform.position = Vector3.Lerp(transform.position, transform.position + transform.right * scaleH * inputH, Time.deltaTime);
        //}

        //#endregion

        #region Attack Part
        if (startAttackFlag)
        {
            innerTime += Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0) && canStartAttackFlag)
        {
            Debug.Log("Detect Combo at innerTime: " + innerTime);
            
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
                if (attackNum < intervalBetweenAttacks.Length && innerTime > intervalBetweenAttacks[attackNum].x && innerTime <= intervalBetweenAttacks[attackNum].y)
                {
                    ++attackNum;
                    Debug.Log("Perform Combo at innerTime: " + innerTime);
                    Debug.Log("Perform Combo: " + attackNum);
                    animController.SetInteger("Attack", attackNum);
                    innerTime = 0;

                    if(attackNum < intervalBetweenAttacks.Length)
                    {
                        Debug.Log("Press After   " + intervalBetweenAttacks[attackNum].x + " seconds.");
                        Debug.Log("Press Befeore " + intervalBetweenAttacks[attackNum].y + " seconds.");
                    }                    

                }
            }
        }

        // 連到最後一招 or 给按的時間過了
        if (
            !isRoll &&
            startAttackFlag && 
            (innerTime > intervalBetweenAttacks[attackNum].y || attackNum > intervalBetweenAttacks.Length - 1)
        )
        {
            Debug.Log("Combo " + attackNum + " Time Expires");
            StartCoroutine(restoreFlag(attackNum));

            attackNum = 0;
            animController.SetInteger("Attack", attackNum);

            canStartAttackFlag = false;
            startAttackFlag = false;
            innerTime = 0;
        }

        #endregion

    }

    IEnumerator PerformRoll()
    {
        float delay = 0.1f;
        yield return new WaitForSeconds(delay);
        animController.SetLayerWeight(1, 0);
        animController.SetBool("Roll", false);

        delay = 1.1f;
        yield return new WaitForSeconds(delay);
        animController.SetLayerWeight(1, 1);

        

        //// StartCoroutine(DelayAdjustWeight("Roll", false, .3f, delegate { isRoll = false; }, .8f, delegate { canRoll = true; canWalk = true; }));

        //float time = 0;

        ////float h = 0.0f;
        ////float v = 1.0f;

        //float h = inputH;
        //float v = inputV;

        ////transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(h, 0f, v), Vector3.up), Time.deltaTime);
        ////Vector3 trans = transform.TransformDirection(new Vector3(inputH, 0f, inputV));
        ////Vector3 newVec = new Vector3(inputH, 0f, inputV);
        ////transform.rotation = Quaternion.LookRotation(trans, Vector3.up);
        

        //while (time < 0.3f)
        //{
        //    time += Time.deltaTime;
        //    float scaleH = (h > 0f) ? rollScaleFactor.w : rollScaleFactor.z;
        //    float scaleV = (v > 0f) ? rollScaleFactor.x : rollScaleFactor.y;
        //    //scaleV = scaleV * (v > 0 ? 1f : -1f);
        //    //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(h, 0f, v), Vector3.up), Time.deltaTime);
        //    transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward * scaleV, Time.deltaTime);
        //    //transform.position = Vector3.Lerp(transform.position, transform.position + transform.right * scaleH, Time.deltaTime);
        //    yield return null;
        //}

        ////yield return new WaitForSeconds(.3f);

        //animController.SetBool("Roll", false);
        //isRoll = false;

        //time = 0;
        //while (time < 0.9f)
        //{
        //    time += Time.deltaTime;
        //    float scaleH = (h > 0f) ? rollScaleFactor.w : rollScaleFactor.z;
        //    float scaleV = (v > 0f) ? rollScaleFactor.x : rollScaleFactor.y;
        //    //scaleV = scaleV * (v > 0 ? 1f : -1f);
        //    transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward * scaleV , Time.deltaTime);
        //    //transform.position = Vector3.Lerp(transform.position, transform.position + transform.right * scaleH, Time.deltaTime);
        //    yield return null;
        //}

        Debug.Log("Done");

        //yield return new WaitForSeconds(.8f);
        canRoll = true;
        canWalk = true;
        isRoll = false;

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

    IEnumerator PreformDrink()
    {
        yield return new WaitForEndOfFrame();

        animController.SetBool("UsingItem", false);

        //float delay = 1.1f;

        //yield return new WaitForSeconds(delay);

        //isDrinking = true;
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
        Debug.Log("End Combo: " + atk);
        if (atk == 4)
        {
            attackNum = 0;
            animController.SetInteger("Attack", attackNum); // Addition Adjust Attack to Sync Leg Layer

            yield return new WaitForSeconds(1.5f);
            canStartAttackFlag = true;

            if (animController.GetLayerWeight(1) != 1f && attackNum == 0)
                animController.SetLayerWeight(1, 1);
        }
        else if (atk == 3)
        {
            // original value: 1.2f, 1.5f, 2f
            float firstTime = .05f;
            float secondTime = 1.0f;
            //float totalTime = 1.35f;

            yield return new WaitForSeconds(firstTime);
            attackNum = 0;
            animController.SetInteger("Attack", attackNum); // Addition Adjust Attack to Sync Leg Layer

            yield return new WaitForSeconds(secondTime - firstTime);
            canStartAttackFlag = true;

            //yield return new WaitForSeconds(totalTime - secondTime);
            if (animController.GetLayerWeight(1) != 1f && attackNum == 0)
                animController.SetLayerWeight(1, 1);
        }
        else if (atk == 2)
        {
            // original value: .2f, .4f
            float firstTime = .2f;
            float totalTime = .3f;

            yield return new WaitForSeconds(firstTime);

            canStartAttackFlag = true;
            //yield return new WaitForSeconds(totalTime - firstTime);
            if (animController.GetLayerWeight(1) != 1f && attackNum == 0)
                animController.SetLayerWeight(1, 1);
        }
        else if (atk == 1)
        {
            // original value: .2f, .6f
            float firstTime = .1f;
            float totalTime = .5f;

            yield return new WaitForSeconds(totalTime);
            canStartAttackFlag = true;

            //yield return new WaitForSeconds(totalTime - firstTime);            
            if (animController.GetLayerWeight(1) != 1f && attackNum == 0)
                animController.SetLayerWeight(1, 1);
        }

        //GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>().canWalk = true;
    }

    // Code Need Refactor
    IEnumerator delayRoutine()
    {
        animController.SetBool("Roll", true);
        yield return new WaitForEndOfFrame();
        animController.SetBool("Roll", false);
    }
}
