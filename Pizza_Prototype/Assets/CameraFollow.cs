using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;

	public float desiredDistance;

	Vector3 ForwardMovement;
	Vector3 UpMovement;
	Vector3 RightMovement;

	Vector3 UpBias;
	Vector3 RightBias;

    Vector3 target_lazyPos;

    HoverBoard playerScript;

	float desiredYOffset = 2;

	float RightSpeed = 0;

    Transform playerBoard;

    Transform AttachedPole;

	// Use this for initialization
	void Start () 
	{
		target_lazyPos = target.transform.position;

		playerScript = target.GetComponent<HoverBoard>();
		if (playerScript == null)
		{
			playerScript = target.GetComponentInParent<HoverBoard>();
		}
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{

		UpdateLazyPos();

        playerBoard = playerScript.transform;
        float playerSpeed = playerBoard != null ? playerBoard.GetComponent<Rigidbody>().velocity.magnitude * 0.5f + 1 : 1;


        transform.LookAt(target_lazyPos);

		Vector3 vecToTarget = (target_lazyPos) - transform.position;

		if (vecToTarget.magnitude > desiredDistance)
		{
            transform.position = target_lazyPos - vecToTarget.normalized * desiredDistance;
			//ForwardMovement = vecToTarget.normalized * (vecToTarget.magnitude - desiredDistance) * 35;
			//UpBiasFromMovement = Vector3.Cross(vecToTarget.normalized, transform.right) * 1.4f;
		
		}
		else
		{
			//ForwardMovement -= ForwardMovement * Time.deltaTime * 3;
			//UpBiasFromMovement = Vector3.zero;
		}


		float currentYOffset = transform.position.y - target_lazyPos.y;

		UpBias = Vector3.Cross(vecToTarget.normalized, transform.right) * (desiredYOffset - currentYOffset) * 5;

        Vector3 RightDirection = Vector3.Cross(vecToTarget.normalized, transform.up);
        //RightBias = Vector3.Cross(vecToTarget.normalized, transform.up) * Vector3.Dot(-vecToTarget.normalized, target.right.normalized) * 15.0f * playerSpeed;



        UpMovement = Vector3.Cross(vecToTarget.normalized, transform.right) * -14 * Input.GetAxis("Vertical_RS");
		RightMovement = Vector3.Cross(vecToTarget.normalized, transform.up) * 25 * Input.GetAxis("Horizontal_RS");

		//if (Mathf.Abs(Input.GetAxis("Horizontal_RS")) > 0.1f)
		//{
		//	RightSpeed += Input.GetAxis("Horizontal_RS") * Time.deltaTime * 6;
		//	RightSpeed = Mathf.Clamp(RightSpeed, -1.5f, 1.5f);
		//}
		//else
		//{
		//	RightSpeed -= RightSpeed * Time.deltaTime * 25;
		//}


		GetComponent<Rigidbody>().velocity = ForwardMovement + UpMovement + RightMovement + UpBias + RightBias;
	}

	void UpdateLazyPos()
	{
		Vector3 vecToTarget = target.transform.position - target_lazyPos;
		Vector3 vecToTarget_noY = new Vector3 (vecToTarget.x, 0, vecToTarget.z);
		target_lazyPos += vecToTarget_noY * Time.deltaTime * 6;

		//if (playerScript == null || playerScript.IsGrounded() == true)
		//{
			target_lazyPos.y += vecToTarget.y * Time.deltaTime * 2;
		//}

	}
}
