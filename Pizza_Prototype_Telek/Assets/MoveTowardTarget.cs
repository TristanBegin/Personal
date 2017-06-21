using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardTarget : MonoBehaviour {

    public float speed;
    public Transform Target;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 goalForward = Vector3.ProjectOnPlane((Target.position - transform.position).normalized, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(goalForward), Time.deltaTime * 30);
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}
}
