using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideMonster : MonoBehaviour {

    public bool isDebug;

    //public GameObject MonsterInstance;

    public AttackAlter attackInstance;
    public bool canHit;
    public float interval;
    public GameObject hitParticlePrefab;
    public GameObject DebugPrefab;

    string[] monsterHitboxTags =
    {
        "TailHitBox",
        "HeadHitBox",
        "BodyHitBox",
        "LegHitBox",
    };

    // For Delaying TriggerEnter when first combo exactly very close to collider
    public bool specialFlag = false;

    void Start () {
        canHit = true;
    }
	
	void Update () {
		if(!attackInstance.startAttackFlag)
        {
            specialFlag = false;

        }
	}

    IEnumerator coolDownTriggerDetection()
    {
        yield return new WaitForSeconds(interval);
        canHit = true;
    }

    IEnumerator DelayHit()
    {
        yield return new WaitForSeconds(.05f);
        canHit = true;

        //specialFlag = true;

        if (attackInstance.attackNum == 1)
        {
            specialFlag = false;
        }
        else
        {
           // yield return new WaitForSeconds(3f);

            specialFlag = false;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        ////No Need For Now
        //if (!specialFlag && canHit && attackInstance.startAttackFlag && other.gameObject.CompareTag("TailHitBox") && attackInstance.attackNum == 1)
        //{
        //    // delay a bit
        //    canHit = false;
        //    StartCoroutine(DelayHit());
        //}

        int matchTagIndex = 0;
        foreach(string str in monsterHitboxTags)
        {
            if (other.gameObject.CompareTag(str))
            {
                break;
            }
            ++matchTagIndex;
        }

        if (canHit && attackInstance.startAttackFlag && matchTagIndex < monsterHitboxTags.Length)
        {
            canHit = false;
            StartCoroutine(coolDownTriggerDetection());
                        
            Vector3 hitPos = other.ClosestPointOnBounds(transform.position);

            Debug.Log("Hit " + monsterHitboxTags[matchTagIndex]);

            GameObject hitParticle = Instantiate(hitParticlePrefab, hitPos, Quaternion.identity);
            hitParticle.transform.localScale = Vector3.one;

            // Special Effect?
            //hitParticle.transform.localScale = Vector3.one * 100f;

            if (isDebug)
            {   
                GameObject g = Instantiate(DebugPrefab, hitPos, Quaternion.identity, other.gameObject.transform.parent);
                
                g.transform.localScale = new Vector3(1f / g.transform.parent.lossyScale.x, 1f / g.transform.parent.lossyScale.y, 1f / g.transform.parent.lossyScale.z);
                g.transform.localScale = Vector3.one * 100f;
                int hitNum = attackInstance.attackNum;

                switch (hitNum)
                {
                    case 1:
                        g.GetComponent<Renderer>().material.color = Color.red;
                        break;
                    case 2:
                        g.GetComponent<Renderer>().material.color = Color.green;
                        break;
                    case 3:
                        g.GetComponent<Renderer>().material.color = Color.blue;
                        break;
                }

                //GameObject.Find("ern001_1").GetComponent<Animator>().SetInteger("Hit", 1);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (canHit)
        {
            //Debug.Log("2");
        }
    }

    private void OnTriggerEXit(Collider other)
    {
        if (canHit && attackInstance.startAttackFlag && other.gameObject.CompareTag("TailHitBox"))
        {
            Debug.Log("3");
        }
    }
}
