using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour {

    public Animator rathianAnim;
    public Transform Player;
    public float StopDistance;
    public float FarDistance;
    public float Speed;
    public float RunSpeed;
    public bool isRun;

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

    [Header("數字越低，代表越有可能在走路後攻擊")]
    public float updateIntervalAfterMove;
    [Header("數字越低，代表越有可能在距離遠時選擇Run")]
    public float updateIntervalAfterFarDistance;

    // 0 < 1 < 2
    // representing move atk fly
    [Header("Move Atk Fly")]
    public Vector3 groundedUpdateThreshold;

    // 0 < 1 < 2
    // Landing, move, atk
    [Header("Landing Move Atk")]
    public Vector3 AirUpdateThreshold;

    int[] attackType = { -1, 1, 2, 3, 4, 5, 6, 7, 8, 9};

    [Header("ATK -1, 1 ~ 9")]
    public Vector2[] attackThreshold;

    public bool isHit;

    [Header("SerializeField")]

    [SerializeField]
    List<state> stateList;

    [SerializeField]
    bool canUpdate;

    [SerializeField]
    float sleepInterval;

    [SerializeField]
    bool grounded;

    [SerializeField]
    bool startAttack;

    [SerializeField]
    bool canUpdateRun;

    [SerializeField]
    float nowDistance;

    [SerializeField]
    int previousAtk;
    
    Quaternion rotateGoal;
    Vector3 positionGoal;

	void Start ()
    {
        canUpdate = true;

        canUpdateRun = true;

        startAttack = false;

        grounded = true;

        stateList = new List<state>();

        stateList.Add(state.MOVE);
        stateList.Add(state.ATK);

        previousAtk = -1;

        // FLY only available after hp less than 75%
    }
	
	void Update ()
    {

        if (canUpdate)
        {
            canUpdate = false;

            //sleepInterval = Random.Range(updateInterval.x, updateInterval.y);

            //StartCoroutine(UpdateUpdateFlag());            

            #region chooseBehaviour
            float min = RandomInterval.x;
            float max = RandomInterval.y;

            float index = Random.Range(min, max);

            Debug.Log(index);

            if (grounded)
            {
                previousState = currentState;

                if (Vector3.Distance(Player.transform.position, transform.position) >= FarDistance && index > updateIntervalAfterFarDistance)
                {
                    // Far From player, should run into player instead
                    Debug.Log("MOVE due to far distance");
                    currentState = state.MOVE;
                }
                else if (currentState == state.MOVE && Vector3.Distance(Player.transform.position, transform.position) <= StopDistance && index > updateIntervalAfterMove)
                {
                    // just after moved, lower posibility to move again, since player may near the monster
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

            }

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

            //Debug.Log(innerTime);

            Quaternion goal = getFacingPlayerRotation();

            transform.rotation = Quaternion.Slerp(transform.rotation, goal, Time.deltaTime);

            yield return null;
        }

        //yield return new WaitForSeconds(delayDueToWalk);
        Debug.Log("Pull Up!");
        rathianAnim.SetInteger("Attack", value);
    }

    IEnumerator delayUpdateCanAttack(float delay)
    {
        yield return new WaitForSeconds(delay + 3f);

        startAttack = false;

        canUpdate = true;
    }

    IEnumerator delaySetAnimAttackToZero()
    {
        yield return new WaitForSeconds(.3f + 3f);

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

            Debug.Log("Do Attack " + attackType[i]);

            StartCoroutine(delayPullUpAtk(attackType[i]));

            //rathianAnim.SetInteger("Attack", attackType[i]);

            StartCoroutine(delaySetAnimAttackToZero());
        }

    }

    void flyUp()
    {
        Debug.Log("FLY");
    }

    void Move()
    {
        rotateGoal = getFacingPlayerRotation();
        positionGoal.x = Player.position.x;
        positionGoal.y = Player.position.y;
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
