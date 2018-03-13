using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSword : MonoBehaviour {

    public float interval;

    Animator animController;
    float time = 0;

	void Start () {
        animController = GetComponent<Animator>();
	}
	
	void Update () {
        if(time > 0)
            time -= Time.deltaTime;

        if (Input.GetKeyUp(KeyCode.R) && time <= 0)
        {
            time = interval;
            animController.SetBool("Draw", !animController.GetBool("Draw"));
        }
	}
}
