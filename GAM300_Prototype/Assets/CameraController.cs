using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	Transform target;
	// Use this for initialization
	void Start () {
		target = GetComponent<Target>().target;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 goalPos = target.position - target.forward * 10 + Vector3.up * 5;

		transform.position += (goalPos - transform.position) * Time.deltaTime * 4;

		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), Time.deltaTime * 20);
	}
}
