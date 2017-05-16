using UnityEngine;
using System.Collections;

public class HoverBoard_Old : MonoBehaviour {

	public LayerMask GroundedMask;

	bool Grounded = false;
	Vector3 GroundedPoint;
	float gravity = 12;
	float desiredFloatingLevel = 1;

	float pushTime = 0;

	Rigidbody myBody;

	Vector3 goalForward;

	float leanValue;

	// Use this for initialization
	void Start () {
		myBody = GetComponent<Rigidbody>();

		goalForward = transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
		CheckGrounded();
        /*
		GetComponent<Rigidbody>().AddForce(Vector3.down * gravity * GetComponent<Rigidbody>().mass);

		if (Grounded)
		{
			Vector3 vecToGround = transform.position - GroundedPoint;
			float force = (desiredFloatingLevel - vecToGround.y);

			myBody.velocity = new Vector3 (myBody.velocity.x, force, myBody.velocity.x);

		}
		*/

        if (pushTime > 0)
        {
            GetComponentInChildren<Renderer>().material.color = Color.green;
            pushTime -= Time.deltaTime;
            myBody.AddForce(transform.forward * pushTime * 45);
        }
        else
        {
            GetComponentInChildren<Renderer>().material.color = Color.white;
        }


        Vector3 ProjectedVelocity = new Vector3(myBody.velocity.x, 0, myBody.velocity.z);
        float offsetFromVelocity = (2 - Vector3.Dot(transform.forward.normalized, ProjectedVelocity.normalized)) / 1.0f;

		myBody.AddForce(-ProjectedVelocity * 0.15f * offsetFromVelocity);

        if (ProjectedVelocity.magnitude > 35)
        {
            myBody.velocity = new Vector3(ProjectedVelocity.normalized.x * 20, myBody.velocity.y, ProjectedVelocity.normalized.z * 20);
        }


        float centripitalForce = 2.5f;
        if (myBody.velocity.magnitude > 0.1f)
        {
            //varries from 1 to 25.
            centripitalForce = 25 / myBody.velocity.magnitude;

            //varries from 1 to 5.
            centripitalForce = Mathf.Sqrt(centripitalForce);


            //if (Input.GetButton("Drift") && Grounded)
            //{
                centripitalForce *= 3 / centripitalForce;
                //myBody.AddForce(-myBody.velocity * 200 * Time.deltaTime);
            //}
        }

        centripitalForce = Mathf.Clamp(centripitalForce, 0.5f, 2.5f);
        

        myBody.angularVelocity = new Vector3(0, leanValue * centripitalForce, 0);




        //myBody.rotation = Quaternion.LookRotation(goalForward);

    }

    void CheckGrounded()
	{
		
		Collider myCol = GetComponent<Collider>();
		RaycastHit hit;
		Grounded = Physics.BoxCast(myCol.bounds.center + Vector3.up * myCol.bounds.extents.x, myCol.bounds.extents, transform.up * -1, out hit, transform.rotation, myCol.bounds.extents.x + 2, GroundedMask);
		//Grounded = Physics.CheckSphere(transform.position, myCol.bounds.extents.x, GroundedMask);
		GroundedPoint = hit.point;
	}

	public void PushForward()
	{
        if (pushTime <= 0)
        {
            pushTime = 1;
        }
	}

	public void Lean(float value)
	{
		leanValue = value;
	}
}
