using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float Acceleration = 10;
	public float Deceleration = 10;
	public float Drag = 10;

	public LayerMask GroundedMask;

	bool AttachedToBoard = false;
	Transform board;

	Rigidbody myBody;

	Camera myCam;

	bool Grounded = false;

	Transform ModAttachment = null;

	// Use this for initialization
	void Start () {
		myBody = GetComponent<Rigidbody>();
		myCam = Camera.main;
	}

	void OnTriggerEnter(Collider hit)
	{
		if (hit.tag == "HoverBoard")
		{
			//transform.parent = hit.transform;
			//transform.localPosition = Vector3.up * GetComponent<Collider>().bounds.extents.y;
			AttachedToBoard = true;
			board = hit.transform;
			myBody.isKinematic = true;
			myBody.useGravity = false;
			myBody.detectCollisions = false;
			//transform.position = new Vector3 (board.position.x, transform.position.y, board.position.z);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		CheckGrounded();

		if (AttachedToBoard == false)
		{
			Movement();
		}
		//else
		//{
		//	//myBody.velocity = Vector3.zero;
		//}

		//if (ModAttachment == null)
		//{
		//	Movement();
		//}
		//else
		//{
		//	if (Input.GetAxis("LeftTrigger") > -0.2f)
		//	{
		//		ModAttachment.GetComponent<Orb_Mod>().Detach();
		//		ModAttachment = null;
		//		myBody.velocity = Vector3.zero;
		//	}
		//}

	}

	void Update()
	{
		if (AttachedToBoard)
		{
			//transform.position = new Vector3 (board.position.x, transform.position.y, board.position.z);
			transform.forward = board.forward;
			transform.parent = board;
			transform.localPosition = new Vector3(0, 1.3f, 0);



			if (Input.GetButtonDown("B_Button"))
			{
				board.GetComponent<HoverBoard_Old>().PushForward();
			}

			board.GetComponent<HoverBoard_Old>().Lean(Input.GetAxis("Horizontal"));
		}

		if (ModAttachment != null)
		{
			transform.position = ModAttachment.position;
		}

		if (Input.GetButtonDown("A_Button") && Grounded)
		{
			myBody.velocity = new Vector3 (myBody.velocity.x, 14.5f, myBody.velocity.z);
		}
	}

	void Movement()
	{
		Vector3 MovementVector = new Vector3 (Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
		//MovementVector.Normalize();



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

		Grounded = Physics.CheckSphere(myCol.bounds.center - new Vector3(0, myCol.bounds.extents.y, 0), myCol.radius*0.9f, GroundedMask) || AttachedToBoard;
	}

	public bool IsGrounded()
	{
		return Grounded || ModAttachment != null;
	}


	void OnTriggerStay(Collider hit)
	{
		if (hit.tag == "OrbMod" && Input.GetAxis("LeftTrigger") < -0.3f)
		{
			Debug.Log("Attach");
			hit.GetComponent<Orb_Mod>().Attach();
			ModAttachment = hit.transform;
		}
	}
		
}
