using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Debugger : MonoBehaviour {

    public AttackAlter attackInstance;
    public CollideMonster collidemMonsterInstance;
    public GameObject[] MonsterHixBox;

    public GameObject[] Axes;

    public bool isDebug = false;

	void Start () {
		
	}

    private void Trigger()
    {
        attackInstance.isDebug = isDebug;
        collidemMonsterInstance.isDebug = isDebug;
        foreach (GameObject g in MonsterHixBox)
        {
            g.GetComponent<Renderer>().enabled = isDebug;
        }
        foreach (GameObject g in Axes)
        {
            Renderer[] r = g.GetComponentsInChildren<Renderer>();
            foreach (Renderer gg in r)
            {
                gg.enabled = isDebug;
            }
        }
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isDebug = !isDebug;
        }

        Trigger();
    }
}
