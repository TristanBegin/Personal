using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FaceTowardVelocity : MonoBehaviour {

    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.forward = GetComponent<Rigidbody>().velocity.normalized;
	}
}
