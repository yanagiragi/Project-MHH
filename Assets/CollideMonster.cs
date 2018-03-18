using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideMonster : MonoBehaviour {

    public AttackAlter attackInstance;
    public bool canHit;
    public float interval;

    //float innerTime = 0;

	// Use this for initialization
	void Start () {
        canHit = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator coolDownTriggerDetection()
    {
        yield return new WaitForSeconds(interval);
        canHit = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canHit && attackInstance.startAttackFlag)
        {
            canHit = false;
            StartCoroutine(coolDownTriggerDetection());
            Debug.Log("1");
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
        if (canHit)
        {
            //Debug.Log("3");
        }
    }
}
