using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.B))
        {
            GetComponent<Animator>().SetBool("Fly", !GetComponent<Animator>().GetBool("Fly"));
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log(GetComponent<Animator>());
            GetComponent<Animator>().SetInteger("Attack", 1);
            StartCoroutine(Attack1());
        }

        KeyCode[] keyCodes = {
             KeyCode.Alpha1,
             KeyCode.Alpha2,
             KeyCode.Alpha3,
             KeyCode.Alpha4,
             KeyCode.Alpha5,
             KeyCode.Alpha6,
             KeyCode.Alpha7,
             KeyCode.Alpha8,
             KeyCode.Alpha9
         };

        for (int i = 0; i < keyCodes.Length; ++i)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                Debug.Log(i + 1);
                GetComponent<Animator>().SetInteger("Attack", i+1);
                StartCoroutine(Attack1());
            }
        }
        
    }

    IEnumerator Attack1()
    {
        yield return new WaitForSeconds(.3f);
        GetComponent<Animator>().SetInteger("Attack", 0);
    }
}
