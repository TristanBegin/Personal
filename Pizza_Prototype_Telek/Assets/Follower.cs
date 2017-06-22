using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {
    public Transform Target;
    Pokable pokable;
	// Use this for initialization
	void Start () {
        pokable = GetComponent<Pokable>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 vecToTarget = Vector3.ProjectOnPlane(Target.transform.position - transform.position, Vector3.up);
		if (pokable.IsPoked && pokable.myPoker.mainTransform != null)
        {
            if (vecToTarget.magnitude > 5)
            {
                transform.forward = vecToTarget.normalized;
                pokable.myPoker.mainTransform.forward = transform.forward;
                pokable.myPoker.mainTransform.position += transform.forward * Time.deltaTime * 6;
            }
        }
        else
        {
            if (vecToTarget.magnitude > 5)
            {
                transform.forward = vecToTarget.normalized;
                transform.position += transform.forward * Time.deltaTime * 6;
            }
        }

        Vector3[] positions = { transform.position, Target.position };
        GetComponent<LineRenderer>().SetPositions(positions);


    }
}
