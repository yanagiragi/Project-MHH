using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour {

    public AudioSource RoarWalkSFX;
    public AudioSource RoarAirSFX;
    public AudioSource Roar3SFX;
    public AudioSource DrawSwordSFX;
    public AudioSource Attack3SFX;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Roar1()
    {
        Roar3SFX.Play();
    }

    public void WalkRoar()
    {
        RoarWalkSFX.Play();
    }

    public void RoarAir()
    {
        RoarAirSFX.Play();
    }

    public void DrawSword()
    {
        Debug.Log("Draw!");
        DrawSwordSFX.Play();
    }

    public void Attack3()
    {
        Debug.Log("Draw!");
        Attack3SFX.Play();
    }
}
