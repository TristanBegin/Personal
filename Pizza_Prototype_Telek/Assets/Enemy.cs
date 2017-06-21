using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider hit) {
        if (hit.tag == "HarmEnemy")
        {
            if (GetComponent<Pokable>() != null)
                GetComponent<Pokable>().Detach();

            Destroy(gameObject);
        }
	}
}
