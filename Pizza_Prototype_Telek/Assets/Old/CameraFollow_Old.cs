using UnityEngine;
using System.Collections;

public class CameraFollow_Old2 : MonoBehaviour {

	public Transform target;

	public float desiredDistance;

	Vector3 ForwardMovement;
	Vector3 UpMovement;
	Vector3 RightMovement;

	Vector3 UpBiasFromMovement;

	Vector3 target_lazyPos;

	Player playerScript;

	float desiredYOffset = 2;

	float RightSpeed = 0;

	// Use this for initialization
	void Start () 
	{
		target_lazyPos = target.transform.position;

		playerScript = target.GetComponent<Player>();
		if (playerScript == null)
		{
			playerScript = target.GetComponentInParent<Player>();
		}
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{

		UpdateLazyPos();


		transform.LookAt(target_lazyPos);

		Vector3 vecToTarget = (target_lazyPos) - transform.position;

		if (vecToTarget.magnitude > desiredDistance)
		{
			ForwardMovement = vecToTarget.normalized * (vecToTarget.magnitude - desiredDistance) * 20;
			//UpBiasFromMovement = Vector3.Cross(vecToTarget.normalized, transform.right) * 1.4f;
		
		}
		else
		{
			ForwardMovement -= ForwardMovement * Time.deltaTime * 3;
			//UpBiasFromMovement = Vector3.zero;
		}


		float currentYOffset = transform.position.y - target_lazyPos.y;

		UpBiasFromMovement = Vector3.Cross(vecToTarget.normalized, transform.right) * (desiredYOffset - currentYOffset) * 5;


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


		GetComponent<Rigidbody>().velocity = ForwardMovement + UpMovement + RightMovement + UpBiasFromMovement;
	}

	void UpdateLazyPos()
	{
		Vector3 vecToTarget = target.transform.position - target_lazyPos;
		Vector3 vecToTarget_noY = new Vector3 (vecToTarget.x, 0, vecToTarget.z);
		target_lazyPos += vecToTarget_noY * Time.deltaTime * 6;

		if (playerScript == null || playerScript.IsGrounded() == true)
		{
			target_lazyPos.y += vecToTarget.y * Time.deltaTime * 2;
		}

	}
}
