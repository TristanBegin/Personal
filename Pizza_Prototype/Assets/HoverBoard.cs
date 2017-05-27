using UnityEngine;
using System.Collections;

public class HoverBoard : MonoBehaviour {

    public LayerMask GroundedMask;

    bool Grounded = false;
    Vector3 GroundedPoint;
    float desiredFloatingLevel = 1;
    float maxSpeed = 50;
    float deceleration = 0.15f;

    float pushTime = 0;

    Rigidbody myBody;

    Vector3 goalForward;

    float leanValue;

    float BrakingPower = 1;

    bool BoostGrounded = false;
    Vector3 GroundNormal;

    Transform AttachedPole = null;

    float originalVelocityMagnitude;

    LineRenderer myLine;

    float timeSinceGrind = 0;

    bool Charging = false;
    float ChargeTime = 0;

    GameObject HookedObject;

    // Use this for initialization
    void Start() {
        myBody = GetComponent<Rigidbody>();

        goalForward = transform.forward;

        
    }

    // Update is called once per frame
    void Update() {
        CheckGrounded();
        CheckInput();

        Vector3 ProjectedVelocity = Vector3.ProjectOnPlane(myBody.velocity, GroundNormal);
        //Vector3 goalForward = myBody.velocity;
        //if (Grounded)
        //{
        //    goalForward = Vector3.ProjectOnPlane(transform.forward, GroundNormal);
        //}
        //
        //transform.forward = Vector3.RotateTowards(transform.forward, goalForward, Time.deltaTime * Vector3.Angle(transform.forward, goalForward) * 0.1f, 0);

        if (pushTime > 0)
        {
            GetComponentInChildren<Renderer>().material.color = Color.green;
            pushTime -= Time.deltaTime * 10;

            if ((ProjectedVelocity.magnitude < maxSpeed || Vector3.Dot(ProjectedVelocity.normalized, transform.forward) < 0.8f))
            {
                if (pushTime > 0.5f)
                {
                    //GetComponentInChildren<Renderer>().material.color = Color.red;
                    myBody.AddForce(transform.forward * (200));
                    if (BoostGrounded)
                    {
                        myBody.AddForce(transform.forward * 50);
                    }
                }
                else
                {
                    //myBody.velocity = Vector3.zero;
                }
            }
        }
        else
        {
            GetComponentInChildren<Renderer>().material.color = Color.white;
        }


        float offsetFromVelocity = (1 - Vector3.Dot(transform.forward.normalized, ProjectedVelocity.normalized)) / 1.0f;

        myBody.AddForce(-ProjectedVelocity.normalized * deceleration * offsetFromVelocity);
        myBody.AddForce(-ProjectedVelocity.normalized * deceleration * BrakingPower * 0.2f);

        //if (ProjectedVelocity.magnitude > maxSpeed)
        //{
        //    myBody.velocity = new Vector3(ProjectedVelocity.normalized.x * maxSpeed, myBody.velocity.y, ProjectedVelocity.normalized.z * maxSpeed);
        //}


        float centripitalForce = 4;
        if (myBody.velocity.magnitude > 0.1f)
        {
            //varries from 1 to 25.
            centripitalForce = 25 / myBody.velocity.magnitude;

            //varries from 1 to 5.
            centripitalForce = Mathf.Sqrt(centripitalForce);

            centripitalForce *= 1.5f;

            if (Grounded == false)
            {
                centripitalForce *= 3 / centripitalForce;
                //myBody.AddForce(-myBody.velocity * 200 * Time.deltaTime);
            }
        }


        centripitalForce = Mathf.Clamp(centripitalForce * 1.5f, 0.5f, 4);


        myBody.angularVelocity = new Vector3(0, leanValue * centripitalForce, 0);

        Vector3 ProjectedForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

        if (Grounded)
        {
            float angleBetween = Vector3.Angle(ProjectedVelocity.normalized, ProjectedForward);
            float velMagnitude = Vector3.Dot(ProjectedVelocity.normalized, ProjectedForward) * ProjectedVelocity.magnitude;
            Vector3 velDirection = ProjectedForward; // Vector3.RotateTowards(ProjectedVelocity.normalized, ProjectedForward, Time.deltaTime * angleBetween * 0.3f, 0);

            Vector3 velocity = velDirection * velMagnitude;
            Vector3 goalVel = new Vector3(velocity.x, myBody.velocity.y, velocity.z);


            myBody.velocity += (goalVel - myBody.velocity) * Time.deltaTime * 4;

        }



        timeSinceGrind += Time.deltaTime;

        if (Charging)
        {
            ChargeTime += Time.deltaTime * 10;
            ChargeTime = Mathf.Clamp(ChargeTime, 0, 10);

            myBody.velocity = Vector3.zero;

            if (Input.GetButton("Y_Button") == false)
            {
                Charging = false;
                myBody.velocity = transform.forward * (ChargeTime + 1) * 10;
                ChargeTime = 0;
            }
        }

    }



    void CheckInput()
    {
        if (Input.GetButtonDown("B_Button"))
        {
            PushForward();
        }

        if (Input.GetButtonDown("A_Button"))
        {
            Jump();
        }

        if (Input.GetButtonDown("X_Button"))
        {
            
        }

        if (Input.GetButtonDown("Y_Button"))
        {
            Charging = true;
        }

            Lean(Input.GetAxis("Horizontal"));

        BrakingPower = 1 - Input.GetAxis("Vertical");
        BrakingPower *= BrakingPower * BrakingPower * BrakingPower * BrakingPower;
        BrakingPower *= BrakingPower;


    }

    void CheckGrounded()
	{
		
		Collider myCol = GetComponent<Collider>();
		RaycastHit hit;
		//Grounded = Physics.BoxCast(myCol.bounds.center + Vector3.up * myCol.bounds.extents.x, myCol.bounds.extents, transform.up * -1, out hit, transform.rotation, myCol.bounds.extents.x + 2, GroundedMask);
        Grounded = Physics.Raycast(myCol.bounds.center, transform.up * -1, out hit, 2, GroundedMask);
        //Grounded = Physics.CheckSphere(transform.position, myCol.bounds.extents.x, GroundedMask);
        if (Grounded)
        {
            GroundedPoint = hit.point;
            BoostGrounded = (hit.transform.gameObject.layer == LayerMask.NameToLayer("BoostGround"));

		    RaycastHit hitFront;
            Physics.Raycast(myCol.bounds.center + transform.forward, transform.up * -1, out hitFront, 2, GroundedMask);

            RaycastHit hitBack;
            Physics.Raycast(myCol.bounds.center - transform.forward, transform.up * -1, out hitBack, 2, GroundedMask);

            GroundNormal = (hit.normal + hitFront.normal + hitBack.normal) / 3.0f;
        }
        else
        {
            GroundNormal = Vector3.up;
        }
    }

    public bool IsGrounded()
    {
        return Grounded;
    }

    void Jump()
    {
        myBody.velocity = new Vector3(myBody.velocity.x, 20, myBody.velocity.z);
    }

	public void PushForward()
	{
        if (pushTime <= 0)
        {
            originalVelocityMagnitude = myBody.velocity.magnitude * 3;
            pushTime = 1;
        }
	}

	public void Lean(float value)
	{
		leanValue = value;
	}
}
