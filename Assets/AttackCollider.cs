using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour {

    public Collider[] Attack1BiteCollider;
    public Collider[] Attack2SprintBiteCollider;
    public Collider[] Attack3LeftStompCollider;
    public Collider[] Attack4RightStompCollider;
    public Collider[] Attack5TwoStompCollider;
    public Collider[] Attack6JumpStompCollider;
    public Collider[] Attack7TackleFailedCollider;

    List<Collider[]> colliderList = new List<Collider[]>();
    List<string> PreviousTagName = new List<string>();

    int prevIndex = -1;
    private Collider[] placeHolder = { };

    void Start () {
        colliderList.Add(placeHolder); // 0
        colliderList.Add(Attack1BiteCollider);
        colliderList.Add(Attack2SprintBiteCollider);
        colliderList.Add(Attack3LeftStompCollider);
        colliderList.Add(Attack4RightStompCollider);
        colliderList.Add(Attack5TwoStompCollider);
        colliderList.Add(Attack6JumpStompCollider);
        colliderList.Add(Attack7TackleFailedCollider);
        colliderList.Add(placeHolder); // 8
        colliderList.Add(placeHolder); // 9
        colliderList.Add(placeHolder); // 10
        colliderList.Add(Attack7TackleFailedCollider); // 11
        colliderList.Add(Attack7TackleFailedCollider); // 12
        colliderList.Add(Attack7TackleFailedCollider); // 13
        colliderList.Add(placeHolder); // 14
    }
	
	public void UpdateCollider (int index)
    {
        if(prevIndex != -1 && index == -1)
        {
            for (int i = 0; i < PreviousTagName.Count; ++i)
            {
                colliderList[prevIndex][i].tag = PreviousTagName[i];
            }

            PreviousTagName.Clear();
        }

        if (index != -1)
        {
            foreach (Collider c in colliderList[index])
            {
                PreviousTagName.Add(c.tag);
                c.tag = "MonsterHitBox";
            }

            prevIndex = index;
        }

        
    }
}
