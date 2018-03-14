using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour {

    public float smooth = 3.0f;
    public GameObject Follow;

    Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = transform.position - Follow.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp( transform.position, Follow.transform.position + offset, Time.deltaTime * smooth);
    }
}
