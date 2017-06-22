using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    bool Attacking = false;
    float AttackProgress = 0;
    float AttackReach = 4;
    float AttackSpeed = 1.6f;

    Poker myPoker;

    Pokable lastPoked;
    
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

        if (myPoker.PokedObject != null)
            lastPoked = myPoker.PokedObject;

        if (Input.GetButtonDown("Y_Button"))
        {
            myPoker.ForcePoke(lastPoked);
        }

        if (Input.GetButton("RB") && lastPoked != null && lastPoked.myPoker != null && lastPoked.myPoker != myPoker)
        {
            lastPoked.myPoker.transform.RotateAround(lastPoked.myPoker.transform.up, Time.deltaTime * 3);
        }

        if (Attacking == true)
        {
            AttackProgress += Time.deltaTime * AttackSpeed;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Sin(AttackProgress * Mathf.PI) * AttackReach);

            if (myPoker.HasObject == true)
                myPoker.TryingToPoke = false;

            if (AttackProgress > 1)
                Attacking = false;
        }
        else
        {
            myPoker.TryingToPoke = false;
        }
        
	}

}
