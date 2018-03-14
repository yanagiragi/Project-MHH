using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkControl : MonoBehaviour {

    public bool isWalk = true;

    [Header("(forward, backward, left, right)")]
    public Vector4 scaleFactor = Vector4.one;

    private float threshold = 0.05f;
    private float epsilon = 0.01f;

    [SerializeField]
    private float inputH;
    [SerializeField]
    private float inputV;
    [SerializeField]
    private bool canWalk = true;
    [SerializeField]
    private bool canRoll = true;


    Animator animController;

    void Start () {
        animController = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");

        isWalk = ((Mathf.Abs(inputH) - epsilon) > threshold) || ((Mathf.Abs(inputV) - epsilon) > threshold);
        isWalk &= animController.GetInteger("Attack") == 0 && animController.GetLayerWeight(1) == 1;
        animController.SetBool("Walk", isWalk);
        animController.SetFloat("InputH", inputH);
        animController.SetFloat("InputV", inputV);



        // 收劍的動作要讓腳sync的上半身layer影響小一點
        if (isWalk)
        {
        
            float scaleH = (inputH > 0f) ? scaleFactor.w : scaleFactor.z;
            float scaleV = (inputV > 0f) ? scaleFactor.x : scaleFactor.y;
            transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward * scaleV * inputV, Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, transform.position + transform.right * scaleH * inputH, Time.deltaTime);
            //transform.position += transform.forward * scaleV * inputV * Time.deltaTime;
            //transform.position += transform.right * scaleH * inputH * Time.deltaTime;
        }
        else
        {
            //animController.SetBool("Walk", isWalk);
        }

        if (isWalk)
        {
            
        }
	}

    IEnumerator delayRoutine()
    {
        animController.SetBool("Roll", true);
        yield return new WaitForEndOfFrame();
        animController.SetBool("Roll", false);
    }
}
