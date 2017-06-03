using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectOnTrigger : MonoBehaviour {

    public GameObject objectToDestroy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider hit)
    {
        if (hit.tag == "Player")
        {
            Destroy(objectToDestroy);
            Destroy(gameObject);
        }
		
	}
}
