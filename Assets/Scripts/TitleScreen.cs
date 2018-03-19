using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour {

    bool isPreformed = true;
    
    public GameObject HixboxModeToggle;
    public GameObject MasterModeToggle;

    void Start () {
	    if(GlobalGameManager.Instance.playCount >= 1)
        {
            HixboxModeToggle.SetActive(true);
        }
        else
        {
            HixboxModeToggle.SetActive(false);
        }

        if (GlobalGameManager.Instance.playCount >= 2)
        {
            MasterModeToggle.SetActive(true);
        }
        else
        {
            MasterModeToggle.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {

        Debug.Log(HixboxModeToggle.GetComponent<UnityEngine.UI.Toggle>().isOn);

        Debug.Log(MasterModeToggle.GetComponent<UnityEngine.UI.Toggle>().isOn);

		if (Input.GetKeyDown(KeyCode.Space) && isPreformed)
        {
            GlobalGameManager.hitBoxMode = HixboxModeToggle.GetComponent<UnityEngine.UI.Toggle>().isOn;
            GlobalGameManager.masterMode = MasterModeToggle.GetComponent<UnityEngine.UI.Toggle>().isOn;
            isPreformed = false;
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainScene");
        }
	}
}
