using UnityEngine;
using System.Collections;

public class MoveTowardTarget : MonoBehaviour {

	public float speed = 25;

	Transform target;

	Vector3 forceDir;

	// Use this for initialization
	void Start () {
		target = GetComponent<Target>().target;
		forceDir = target.position - transform.position;
		forceDir.Normalize();
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody>().AddForce(forceDir * speed);

		if (GetComponent<Rigidbody>().velocity.magnitude > 15)
		{
			GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * 15;
		}

		forceDir += (target.position - transform.position).normalized * Time.deltaTime;
		forceDir.Normalize();

		//transform.RotateAround(transform.position, Vector3.up, Vector3.Angle(transform.forward, (target.position - transform.position).normalized) * Time.deltaTime * 5);
	}
}
