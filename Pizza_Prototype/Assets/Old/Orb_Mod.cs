using UnityEngine;
using System.Collections;

public class Orb_Mod : MonoBehaviour {


	public Transform Orb;

	public Vector3 Offset;

	Transform cam;

	Vector3 Orb_vel;

	protected bool PlayerAttached = false;

	// Use this for initialization
	void Start () {
		cam = Camera.main.transform;
		Orb_vel = Orb.GetComponent<Rigidbody>().velocity;

	}
	
	// Update is called once per frame
	void LateUpdate () {

		Vector3 OrbForward = cam.forward;

		if (OrbForward.magnitude > 0.5f)
		{
			Orb_vel = Vector3.RotateTowards(Orb_vel, OrbForward, Time.deltaTime, Time.deltaTime);
		}

		Vector3 Direction = new Vector3 (Orb_vel.x, 0, Orb_vel.z);

		Quaternion FromToRotation = Quaternion.FromToRotation(Vector3.forward, Direction);

		Vector3 mod_Offset = FromToRotation * Offset;
		transform.up = Direction.normalized;

		transform.position = Orb.position + mod_Offset;

	}

	public void Attach()
	{
		PlayerAttached = true;
	}

	public void Detach()
	{
		PlayerAttached = false;
	}
}
