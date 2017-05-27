using UnityEngine;
using System.Collections;

public class ATV_Controls : MonoBehaviour {

	public LayerMask groundedMask;

	Rigidbody myBody;

	Vector3 angVel;

	Transform mesh;

	bool Grounded;

	float upAngle = 0;

	// Use this for initialization
	void Start () {
		myBody = GetComponent<Rigidbody>();

		mesh = transform.FindChild("MeshBody");
	}
	
	// Update is called once per frame
	void Update () {

		CalculateGrounded();

		Debug.Log(Grounded);

		if (Grounded)
		{
			// Throttle
			myBody.AddForce(transform.forward * 5000 * Time.deltaTime * Input.GetAxis("Gas"));

			//Friction
			myBody.AddForce(25 * Time.deltaTime * -myBody.velocity);

			if (myBody.velocity.magnitude > 25)
			{
				myBody.velocity = myBody.velocity.normalized * 25;
			}
		}






			
		float centripitalForce = 2.5f;
		if (myBody.velocity.magnitude > 0.1f)
		{
			//varries from 1 to 25.
			centripitalForce = 25 / myBody.velocity.magnitude;

			//varries from 1 to 5.
			centripitalForce = Mathf.Sqrt(centripitalForce);


			if (Input.GetButton("Drift") && Grounded)
			{
				centripitalForce *= 5 / centripitalForce;
				myBody.AddForce(-myBody.velocity * 200 * Time.deltaTime );
			}
		}

		centripitalForce = Mathf.Clamp(centripitalForce, 0.5f, 2.5f);




		myBody.angularVelocity = new Vector3(0, Input.GetAxis("Horizontal") * centripitalForce, 0);



		//Debug.Log(myBody.velocity.magnitude);


		float actual_z = mesh.eulerAngles.z;
		float mod_z = actual_z;
		if (mod_z > 180)
		{
			mod_z -= 360;
		}
		//myBody.angularVelocity -= new Vector3 (0, 0, mod_z * 0.04f);

		//mesh.eulerAngles += new Vector3 (0, 0, -Input.GetAxis("Horizontal") * 50 * Time.deltaTime * centripitalForce);
		//mesh.eulerAngles -= new Vector3 (0, 0, 5 * Time.deltaTime * mod_z);

		mesh.RotateAround(mesh.position, mesh.forward, -Input.GetAxis("Horizontal") * 50 * Time.deltaTime * centripitalForce);
		mesh.RotateAround(mesh.position, mesh.forward, -5 * Time.deltaTime * mod_z);


		float actual_x = mesh.eulerAngles.x;
		float mod_x = actual_x;
		if (mod_x > 180)
		{
			mod_x -= 360;
		}

		mesh.RotateAround(mesh.position, mesh.right, upAngle - mod_x);
	}


	void CalculateGrounded()
	{
		Grounded = Physics.Raycast(transform.position, Vector3.down, 1, groundedMask);
	}

	public bool GetGrounded()
	{
		return Grounded;
	}

	public void Jump()
	{
		if (Grounded)
		{
			myBody.velocity += new Vector3 (0, 20, 0);
		}
	}
}
