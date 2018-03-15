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
        if(Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log(GetComponent<Animator>());
            GetComponent<Animator>().SetInteger("Attack", 1);
            StartCoroutine(Attack1());
        }
	}

    IEnumerator Attack1()
    {
        yield return new WaitForSeconds(.1f);
        GetComponent<Animator>().SetInteger("Attack", 0);
    }
}
