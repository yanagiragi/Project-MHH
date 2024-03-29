﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour {

    public AttackCollider atkColliderInstance;
    public Animator rathianAnim;
    public Transform Player;

    public bool isDebug;

    public bool isRun;
    public bool isHit;

    public enum state{
        MOVE,
        ATK,
        FLY,
        LENGTH
    };
    
    public state currentState = state.LENGTH;
    public state previousState = state.LENGTH;

    public Vector2 updateInterval;

    public Vector2 RandomInterval;

    [Header("=== Grounded ===")]

        public float StopDistance;
        public float FarDistance;
        public float Speed;
        public float RunSpeed;

        [Header("數字越低，代表越有可能在走路後攻擊")]
        public float updateIntervalAfterMove;
        [Header("數字越低，代表越有可能在距離遠時選擇Run")]
        public float updateIntervalAfterFarDistance;

        // 0 < 1 < 2
        // representing move atk fly
        [Header("Move Atk Fly")]
        public Vector3 groundedUpdateThreshold;

        int[] attackType = { -1, 1, 2, 3, 4, 5, 6, 7, 8, 9};

        [Header("ATK -1, 1 ~ 9")]
        public Vector2[] attackThreshold;

        [SerializeField]
        bool canUpdateRun;

    [Header("=== On-the-Air ===")]

        public float StopDistanceAir;
        public float FarDistanceAir;
        public float SpeedAir;
        public float RunSpeedAir;

        [Header("數字越低，代表越有可能在走路後攻擊")]
        public float updateIntervalAfterMoveAir;
        [Header("數字越低，代表越有可能在距離遠時選擇Run")]
        public float updateIntervalAfterFarDistanceAir;

        // 0 < 1 < 2
        // Landing, move, atk
        [Header("Landing Move Atk")]
        public Vector3 AirUpdateThreshold;

        int[] attackTypeAir = { 11, 12, 13, 14};

        [Header("ATK 11 ~ 14")]
        public Vector2[] attackThresholdAir;

        [SerializeField]
        bool canUpdateSlide;

    [Header(" === SerializeField ===")]
    
        [SerializeField]
        List<state> stateList;

        [SerializeField]
        bool canUpdate;

        [SerializeField]
        float sleepInterval;

        [SerializeField]
        bool grounded;

        public bool startAttack;

        [SerializeField]
        bool canUpdateFly;

        [SerializeField]
        float nowDistance;

        [SerializeField]
        int previousAtk;
    
    Quaternion rotateGoal;
    Vector3 positionGoal;

	void Start ()
    {
        canUpdate = true;

        canUpdateFly = true;

        canUpdateRun = true;

        canUpdateSlide = true;

        startAttack = false;

        grounded = true;

        stateList = new List<state>();

        stateList.Add(state.MOVE);
        stateList.Add(state.ATK);

        previousAtk = -1;

    }
	
	void Update ()
    {

        if (canUpdate)
        {
            canUpdate = false;

            #region chooseBehaviour
            float min = RandomInterval.x;
            float max = RandomInterval.y;

            float index = Random.Range(min, max);

            if (grounded)
            {
                #region grounded_Action_Selection

                previousState = currentState;

                if (Vector3.Distance(Player.transform.position, transform.position) >= FarDistance && index > updateIntervalAfterFarDistance)
                {
                    // Far From player, should run into player instead
                    if (isDebug)
                        Debug.Log("MOVE due to far distance");
                    currentState = state.MOVE;
                }
                else if (currentState == state.MOVE && Vector3.Distance(Player.transform.position, transform.position) <= StopDistance && index > updateIntervalAfterMove)
                {
                    // just after moved, lower posibility to move again, since player may near the monster
                    if(isDebug)
                        Debug.Log("ATK due to prev MOVE");
                    currentState = state.ATK;
                }
              
                else if (index < groundedUpdateThreshold[0])
                {
                    currentState = state.MOVE;
                }

                else if (index < groundedUpdateThreshold[1])
                {
                    currentState = state.ATK;
                }
                else
                {
                    currentState = state.FLY;
                }
                #endregion

            }
            else
            {
                #region Air_Action_Selection

                previousState = currentState;

                if (Vector3.Distance(Player.transform.position, transform.position) >= FarDistanceAir && index > updateIntervalAfterFarDistanceAir)
                {
                    // Far From player, should run into player instead
                    Debug.Log("MOVE due to far distance");
                    currentState = state.MOVE;
                }
                else if (currentState == state.MOVE && Vector3.Distance(Player.transform.position, transform.position) <= StopDistanceAir && index > updateIntervalAfterMoveAir)
                {
                    // just after moved, lower posibility to move again, since player may near the monster
                    Debug.Log("ATK due to prev MOVE");
                    currentState = state.ATK;
                }
                else if (index < AirUpdateThreshold[0])
                {
                    currentState = state.FLY;
                }

                else if (index < AirUpdateThreshold[1])
                {
                    currentState = state.MOVE;
                }
                else
                {
                    currentState = state.ATK;
                }

                if(previousState == state.FLY)
                {
                    currentState = state.ATK;
                }

                #endregion
            }

            if(isDebug)
                Debug.Log("NState = " + currentState);

            #endregion
        }

        if(!canUpdate)
        {
            if (grounded)
            {
                switch (currentState)
                {
                    case state.MOVE:
                        Move();
                        break;
                    case state.ATK:
                        AttackPlayer();
                        break;
                    case state.FLY:
                        flyUp();
                        break;
                    default:
                        Debug.LogError("No Such State");
                        break;
                }
            }
            else
            {
                switch (currentState)
                {
                    case state.MOVE:
                        MoveAir();
                        break;
                    case state.ATK:
                        AttackPlayerAir();
                        break;
                    case state.FLY:
                        if(canUpdateFly)
                            Landing();
                        break;
                    default:
                        Debug.LogError("No Such State");
                        break;
                }
            }
        }

        // Debug.Log(Vector3.Distance(transform.position, Player.transform.position));
    }

    IEnumerator delayStartSlide()
    {
        float runSpeed = RunSpeedAir;
        float speed = SpeedAir;

        // limit Speed Before Start Sliding
        SpeedAir = RunSpeedAir = 0.0f;
        
        isRun = true;

        while (rathianAnim.GetCurrentAnimatorStateInfo(0).IsName("StartSlide"))
        {
            yield return null;
        }

        yield return new WaitForSeconds(5.5f);

        RunSpeedAir = runSpeed;
        SpeedAir = speed;
    }

    void MoveAir()
    {
        rotateGoal = getFacingPlayerRotation();
        positionGoal.x = Player.position.x;
        positionGoal.y = transform.position.y;
        positionGoal.z = Player.position.z;

        nowDistance = Vector3.Distance(positionGoal, transform.position);

        if (canUpdateSlide)
        {
            // update goal           

            // if so, preform run anim until next tick
            if(nowDistance > FarDistance || nowDistance > FarDistance)
            {
                StartCoroutine(delayStartSlide());
            }

            canUpdateSlide = false;

            nowDistance = Vector3.Distance(positionGoal, transform.position);

        }

        // Start Rotate
        // Can't use transform.LookAt since there is an offset between root and animations, Use Below instead
        transform.rotation = Quaternion.Slerp(transform.rotation, rotateGoal, Time.deltaTime);

        // Start Moving
        nowDistance = Vector3.Distance(positionGoal, transform.position);

   
        if (nowDistance > StopDistanceAir)
        {
            transform.position = Vector3.Lerp(transform.position, positionGoal, Time.deltaTime * (isRun ? RunSpeedAir : SpeedAir));

            if (isRun)
            {
                rathianAnim.SetBool("Run", true);
                //Vector3 angles = rotateGoal.eulerAngles;
                //angles = angles.normalized;

                //Debug.Log(angles);

                //rathianAnim.SetFloat("InputH", angles.y);
            }
            else
            {
                rathianAnim.SetBool("Run", false);
            }
        }
        else
        {
            canUpdateSlide = true;

            canUpdate = true;

            isRun = false;

            rathianAnim.SetBool("Run", false);
        }

    }

    void AttackPlayerAir()
    {
        if (!startAttack)
        {
            startAttack = true;
            
            // start Attack
            float atkIndex = Random.Range(RandomInterval.x, RandomInterval.y);

            int i;
            for (i = 0; i < attackThresholdAir.Length; ++i)
            {
                if (atkIndex < attackThresholdAir[i].x)
                {
                    break;
                }
            }

            if (i == previousAtk)
            {
                if (i < attackThresholdAir.Length - 1)
                    ++i;
                else
                    i = 0;
            }

            previousAtk = i;

            if (isDebug)
                Debug.Log(i + ", " + attackThresholdAir.Length);

            StartCoroutine(delayUpdateCanAttack(attackThresholdAir[i].y));

            if (isDebug)
                Debug.Log("Do Attack " + attackTypeAir[i]);

            StartCoroutine(delayPullUpAtk(attackTypeAir[i]));

            StartCoroutine(delaySetAnimAttackToZero(i + 10));
        }
    }

    IEnumerator delayRestoreCanUpdateAfterFly()
    {
        yield return new WaitForSeconds(6f);

        canUpdate = true;
        canUpdateFly = true;
        Debug.Log("Pull Up Fly Flags");
    }

    IEnumerator delayRestoreFlagsAfterLand()
    {
        yield return new WaitForSeconds(6f);

        Debug.Log("Pull Up Land Flags");
        canUpdateFly = true;
        canUpdate = true;
    }

    void flyUp()
    {
        if (grounded && canUpdateFly)
        {
            if (isDebug)
                Debug.Log("FLY");

            isRun = false;
            grounded = false;
            canUpdate = false;
            canUpdateFly = false;

            rathianAnim.SetBool("Walk", false);
            rathianAnim.SetBool("Run", false);
            rathianAnim.SetInteger("Attack", 0);
            rathianAnim.SetBool("Fly", true);

            StartCoroutine(delayRestoreCanUpdateAfterFly());
        }

    }

    void Landing()
    {
        if (!grounded && canUpdateFly)
        {
            if (isDebug)
                Debug.Log("LAND");

            isRun = false;
            grounded = true;
            canUpdate = false;
            canUpdateFly = false;

            rathianAnim.SetBool("Walk", false);
            rathianAnim.SetBool("Run", false);
            rathianAnim.SetInteger("Attack", 0);
            rathianAnim.SetBool("Fly", false);

            StartCoroutine(delayRestoreFlagsAfterLand());
        }
    }

    IEnumerator UpdateUpdateFlag()
    {
        yield return new WaitForSeconds(sleepInterval);

        canUpdate = true;
    }

    IEnumerator delayPullUpAtk(int value)
    {
        
        float innerTime = 0f;

        while(innerTime < 3f)
        {
            innerTime += Time.deltaTime;
            
            Quaternion goal = getFacingPlayerRotation();

            transform.rotation = Quaternion.Slerp(transform.rotation, goal, Time.deltaTime);

            yield return null;
        }

        if (isDebug)
            Debug.Log("Pull Up!");

        atkColliderInstance.UpdateCollider(-1);
        rathianAnim.SetInteger("Attack", value);
    }

    IEnumerator delayUpdateCanAttack(float delay)
    {
        yield return new WaitForSeconds(delay + 3f);

        startAttack = false;

        canUpdate = true;

        // Restore Collider State
        atkColliderInstance.UpdateCollider(-1);
    }

    IEnumerator delaySetAnimAttackToZero(int index)
    {
        yield return new WaitForSeconds(.3f + 3f);

        atkColliderInstance.UpdateCollider(index);

        rathianAnim.SetInteger("Attack", 0);
    }

    IEnumerator clearAnimRunFlag()
    {
        bool isPreforming = rathianAnim.GetBool("Run") & rathianAnim.GetBool("Walk");

        if (isPreforming)
        {
            rathianAnim.SetBool("Run", false);
            rathianAnim.SetBool("Walk", false);
            yield return null;
        }
        else
        {
            // Preform Slice of walk animation when preprocessing rotation for delayPullUpAtk
            rathianAnim.SetBool("Run", false);

            rathianAnim.SetBool("Walk", true);

            yield return new WaitForSeconds(.5f);

            rathianAnim.SetBool("Walk", false);
        }
    }

    void AttackPlayer()
    {
        if(!startAttack)
        {
            if (isDebug)
                Debug.Log("ATK");

            // clean animation flags
            StartCoroutine(clearAnimRunFlag());

            startAttack = true;            
            

            // start Attack
            float atkIndex = Random.Range(RandomInterval.x, RandomInterval.y);

            int i;
            for(i = 0; i < 10; ++i)
            {
                if (atkIndex < attackThreshold[i].x)
                {
                    break;
                }
            }

            if(i == previousAtk)
            {
                if (i != attackThreshold.Length - 1)
                    ++i;
                else
                    i = 0;
            }

            previousAtk = i;

            StartCoroutine(delayUpdateCanAttack(attackThreshold[i].y));

            if (isDebug)
                Debug.Log("Do Attack " + attackType[i]);

            StartCoroutine(delayPullUpAtk(attackType[i]));

            StartCoroutine(delaySetAnimAttackToZero(i));
        }

    }   

    void Move()
    {
        rotateGoal = getFacingPlayerRotation();
        positionGoal.x = Player.position.x;
        positionGoal.y = transform.position.y;
        positionGoal.z = Player.position.z;

        nowDistance = Vector3.Distance(positionGoal, transform.position);

        if (canUpdateRun)
        {
            // update goal           

            // if so, preform run anim until next tick
            isRun = nowDistance > FarDistance;

            canUpdateRun = false;
        }

        // Start Rotate
        // Can't use transform.LookAt since there is an offset between root and animations, Use Below instead
        transform.rotation = Quaternion.Slerp(transform.rotation, rotateGoal, Time.deltaTime);

        // Start Moving
        nowDistance = Vector3.Distance(positionGoal, transform.position);

        if (nowDistance > FarDistance)
        {
            isRun = true;
        }

        if (nowDistance > StopDistance)
        {
            transform.position = Vector3.Lerp(transform.position, positionGoal, Time.deltaTime * (isRun ? RunSpeed : Speed));

            if (isRun)
            {
                rathianAnim.SetBool("Run", true);
                rathianAnim.SetBool("Walk", false);
            }
            else
            {
                rathianAnim.SetBool("Run", false);
                rathianAnim.SetBool("Walk", true);
            }
        }
        else
        {
            canUpdateRun = true;

            canUpdate = true;

            if (Quaternion.Angle(transform.rotation, rotateGoal) < 0.1f)
            {
                rathianAnim.SetBool("Run", false);
                rathianAnim.SetBool("Walk", false);
            }
            else
            {
                rathianAnim.SetBool("Run", false);
                rathianAnim.SetBool("Walk", true);
            }
        }
    }

    Quaternion getFacingPlayerRotation()
    {
        Vector3 positionDiff = transform.position - Player.position;

        Quaternion q = Quaternion.LookRotation(positionDiff) * Quaternion.Euler(0f, -90f, 0f);

        return q;
    }
}
