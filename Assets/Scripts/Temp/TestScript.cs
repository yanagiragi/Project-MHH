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
            GetComponent<Animator>().SetBool("Run", !GetComponent<Animator>().GetBool("Run"));
            StartCoroutine(Attack1());
        }

        GetComponent<Animator>().SetFloat("InputH", Input.GetAxis("Horizontal"));

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

        bool switchHit = Input.GetKey(KeyCode.X);
        bool switchAttack = Input.GetKey(KeyCode.Z);

        for (int i = 0; i < keyCodes.Length; ++i)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                if (switchHit)
                {
                    GetComponent<Animator>().SetInteger("Hit", i + 1);
                    Debug.Log(i + 1);

                    StartCoroutine(Hit1());
                }
                else if(switchAttack)
                {
                    Debug.Log(i + 11);

                    GetComponent<Animator>().SetInteger("Attack", i + 11);
                    StartCoroutine(Attack1());
                }
                else
                {
                    GetComponent<Animator>().SetInteger("Attack", i + 1);
                    StartCoroutine(Attack1());
                }
                
            }
        }
        
    }

    IEnumerator Hit1()
    {
        yield return new WaitForSeconds(.3f);
        GetComponent<Animator>().SetBool("Run", !GetComponent<Animator>().GetBool("Run"));
    }

    IEnumerator Attack1()
    {
        yield return new WaitForSeconds(.3f);
        GetComponent<Animator>().SetInteger("Attack", 0);
    }
}
