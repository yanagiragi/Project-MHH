using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //float max = 11.58f;
        float max = 11.58f * 2;
        float inputH = Input.GetAxis("Horizontal");

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(8.879001f, inputH * max, 0)), Time.deltaTime);


    }
}
