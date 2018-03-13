using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSword : MonoBehaviour {

    public GameObject SwordInHip;
    public GameObject SwordInHand;

    Animator animController;

    // Use this for initialization
    void Start () {
        animController = GetComponent<Animator>();
        SwordInHip.SetActive(true);
        SwordInHand.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (animController.GetCurrentAnimatorStateInfo(0).IsName("DrawSword2") && SwordInHip.activeSelf)
        {
            SwordInHip.SetActive(false);
            SwordInHand.SetActive(true);
        }
        else if (animController.GetCurrentAnimatorStateInfo(0).IsName("Sheath2"))
        {
            SwordInHip.SetActive(true);
            SwordInHand.SetActive(false);
        }
    }
}
