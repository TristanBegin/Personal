using UnityEngine;
using System.Collections;

public class shootAtTarget : MonoBehaviour {


	public GameObject bullet;
	Transform target;
	public float delay;
	public float bulletSpeed = 15;

	float currDelay;

	// Use this for initialization
	void Start () {
		currDelay = delay;
		target = GetComponent<Target>().target;
	}
	
	// Update is called once per frame
	void Update () {

		currDelay -= Time.deltaTime;

		if (currDelay < 0)
		{
			currDelay = delay;

			GameObject clone = (GameObject)Instantiate(bullet, transform.position - Vector3.up*1.5f, transform.rotation);
			Vector3 dir = (target.position - transform.position).normalized;
			clone.GetComponent<Rigidbody>().velocity = new Vector3(dir.x, 0, dir.z) * bulletSpeed;
		}
	}
}
