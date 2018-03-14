using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDie : MonoBehaviour {

    public bool isHit = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.C) && !isHit)
        {
            isHit = true;
            GetComponent<Animator>().SetBool("Hit", true);
            StartCoroutine(aaa());
        }
	}

    IEnumerator aaa()
    {
        yield return new WaitForSeconds(.2f);
        GetComponent<Animator>().SetBool("Hit", false);

        yield return new WaitForSeconds(3.8f);
        isHit = false;
    }
}
