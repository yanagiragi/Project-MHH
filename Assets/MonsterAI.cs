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

    [SerializeField]
    bool canUpdateGoal = false;

    [SerializeField]
    float nowDistance;

    Quaternion rotateGoal;
    Vector3 positionGoal;

	void Start ()
    {
        // For Develop
        canUpdateGoal = true;
        
    }
	
	void Update ()
    {

        // 如果彼此離的夠進 -> 攻擊
        // wait Random Seconds
        // 如果離得太遠 -> 移動
        // 飛行應該會是自己另一個routine

        tick();
	}

    void tick()
    {
        rotateGoal = getFacingPlayerRotation();
        positionGoal.x = Player.position.x;
        positionGoal.y = Player.position.y;
        positionGoal.z = Player.position.z;

        nowDistance = Vector3.Distance(positionGoal, transform.position);

        if (canUpdateGoal)
        {
            // update goal           

            // if so, preform run anim until next tick
            isRun = nowDistance > FarDistance;

            canUpdateGoal = false;
        }

        // Start Rotate
        // Can't use transform.LookAt since there is an offset between root and animations, Use Below instead
        transform.rotation = Quaternion.Slerp(transform.rotation, rotateGoal, Time.deltaTime);

        // Start Moving
        nowDistance = Vector3.Distance(positionGoal, transform.position);

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
            canUpdateGoal = true;
            rathianAnim.SetBool("Run", false);
            rathianAnim.SetBool("Walk", false);
        }
    }

    Quaternion getFacingPlayerRotation()
    {
        Vector3 positionDiff = transform.position - Player.position;

        Quaternion q = Quaternion.LookRotation(positionDiff) * Quaternion.Euler(0f, -90f, 0f);

        return q;
    }
}
