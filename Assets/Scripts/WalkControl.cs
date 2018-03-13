using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkControl : MonoBehaviour {

    public bool isWalk = true;
    public float scale = 1;

    Animator animController;

    void Start () {
        animController = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        // 收劍的動作要讓腳sync的上半身layer影響小一點
        if (Input.GetKey(KeyCode.W))
        {
            isWalk = true;
            transform.position += transform.forward * scale;
            animController.SetBool("Walk", isWalk);
        }
        else
        {
            isWalk = false;
            animController.SetBool("Walk", isWalk);
        }

        if (isWalk)
        {
            
        }
	}
}
