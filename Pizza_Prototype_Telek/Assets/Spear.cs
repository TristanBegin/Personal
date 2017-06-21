using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    bool Attacking = false;
    float AttackProgress = 0;
    float AttackReach = 4;
    float AttackSpeed = 1;

    Poker myPoker;
    
	// Use this for initialization
	void Start ()
    {
        myPoker = GetComponentInChildren<Poker>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetAxis("RightTrigger") < -0.1f && Attacking == false)
        {
            Attacking = true;
            AttackProgress = 0;
            if (myPoker.HasObject == false)
                myPoker.TryingToPoke = true;
        }

        if (Input.GetButtonDown("X_Button"))
        {
            myPoker.Detach();
        }

        if (Attacking == true)
        {
            AttackProgress += Time.deltaTime * AttackSpeed;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Sin(AttackProgress * Mathf.PI) * AttackReach);
            if (AttackProgress > 1)
                Attacking = false;
        }
        else
        {
            myPoker.TryingToPoke = false;
        }
        
	}

}
