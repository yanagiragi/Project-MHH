using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour {

    public GameObject Follow;

    Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = transform.position - Follow.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Follow.transform.position + offset;
    }
}
