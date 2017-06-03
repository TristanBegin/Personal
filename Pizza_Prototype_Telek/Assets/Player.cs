using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float Acceleration = 13;
	public float Deceleration = 10;
	public float Drag = 10;

	public LayerMask GroundedMask;

	Rigidbody myBody;

	Camera myCam;

	bool Grounded = false;

    bool WasNotRolling = true;

	// Use this for initialization
	void Start ()
    {
		myBody = GetComponent<Rigidbody>();
		myCam = Camera.main;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		CheckGrounded();

        if (Catcher.Rolling)
        {
            RollingMovement();
        }
        else
        {
            Movement();
        }

        if (Input.GetButtonDown("A_Button") && Grounded)
        {
            myBody.velocity = new Vector3(myBody.velocity.x, 14.5f, myBody.velocity.z);
        }
    }

	void Update()
	{
        
		
	}

    void RollingMovement()
    {
        if (WasNotRolling)
        {
            WasNotRolling = false;
            transform.forward = Vector3.ProjectOnPlane(myCam.transform.forward, transform.up);
        }

        Vector3 ProjectedVelocity = Vector3.ProjectOnPlane(myBody.velocity, transform.up);

        if ((ProjectedVelocity.magnitude < 25 || Vector3.Dot(ProjectedVelocity.normalized, transform.forward) < 0.8f))
        {
            myBody.AddForce(transform.forward * (200));
        }


        float centripitalForce = 4;
        if (myBody.velocity.magnitude > 0.1f)
        {
            //varries from 1 to 25.
            centripitalForce = 25 / myBody.velocity.magnitude;

            //varries from 1 to 5.
            centripitalForce = Mathf.Sqrt(centripitalForce);

            if (Grounded == false)
            {
                centripitalForce *= 3 / centripitalForce;
                //myBody.AddForce(-myBody.velocity * 200 * Time.deltaTime);
            }
        }


        centripitalForce = Mathf.Clamp(centripitalForce * 1.5f, 0.5f, 4);

        float leanValue = Input.GetAxis("Horizontal");
        myBody.angularVelocity = new Vector3(0, leanValue * centripitalForce, 0);

        Vector3 ProjectedForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

        //if (Grounded)
        //{
            float angleBetween = Vector3.Angle(ProjectedVelocity.normalized, ProjectedForward);
            float velMagnitude = Vector3.Dot(ProjectedVelocity.normalized, ProjectedForward) * ProjectedVelocity.magnitude;
            Vector3 velDirection = ProjectedForward; 
        
            Vector3 velocity = velDirection * velMagnitude;
            Vector3 goalVel = new Vector3(velocity.x, myBody.velocity.y, velocity.z);
        
        
            myBody.velocity += (goalVel - myBody.velocity) * Time.deltaTime * 4;

        //}
        //RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, 2, GroundedMask) && myBody.velocity.y < 15)
        {
            myBody.AddForce(transform.up * 200);
        }


    }

    void Movement()
	{
        WasNotRolling = true;

		Vector3 MovementVector = new Vector3 (Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

		//Modify later to account for slopes.
		Vector3 camForward = new Vector3 (myCam.transform.forward.x, 0, myCam.transform.forward.z);

        Quaternion FromToRotation = Quaternion.FromToRotation(Vector3.forward, camForward);

        
        Vector3 DesiredDirection = FromToRotation * MovementVector;

        
		if (MovementVector.magnitude > 0.1f)
		{
			myBody.rotation = Quaternion.RotateTowards(myBody.rotation, Quaternion.LookRotation(DesiredDirection), Time.deltaTime * 10 * Quaternion.Angle(myBody.rotation, Quaternion.LookRotation(DesiredDirection)));
		}


		Vector3 ProjectedVelocity = new Vector3(myBody.velocity.x, 0, myBody.velocity.z);

		if (MovementVector.magnitude > 0.1f)
		{
			myBody.AddForce(DesiredDirection * MovementVector.magnitude * Acceleration);
			myBody.AddForce(-1 * Drag * ProjectedVelocity);
		}
		else if (ProjectedVelocity.magnitude > 0.1f)
		{
			
			myBody.AddForce(-1 * ProjectedVelocity * Deceleration);
		}
		else
		{
			myBody.velocity = new Vector3 (0, myBody.velocity.y, 0);
		}




	}

	void CheckGrounded()
	{
		CapsuleCollider myCol = GetComponent<CapsuleCollider>();

		Grounded = Physics.CheckSphere(myCol.bounds.center - new Vector3(0, myCol.bounds.extents.y, 0), myCol.radius*0.9f, GroundedMask);
	}

	public bool IsGrounded()
	{
		return Grounded;
	}


	void OnTriggerStay(Collider hit)
	{
		
	}
		
}
