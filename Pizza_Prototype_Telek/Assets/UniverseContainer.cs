using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UniverseContainer : MonoBehaviour {

    public static event Action PlayerEnterUniverse;
    public static event Action PlayerExitUniverse;

    GameObject player;
    bool playerInside;

    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 vecToPlayer = (player.transform.position - transform.position);
        if (vecToPlayer.magnitude < GetComponent<SphereCollider>().bounds.extents.x)
        {
            if (playerInside == false)
            {
                playerInside = true;
                PlayerEnterUniverse();
            }
        }
        else
        {
            if (playerInside == true)
            {
                playerInside = false;
                PlayerExitUniverse();
            }
        }
		
	}
}
