using UnityEngine;
using System.Collections;

public class Sword_Controls : MonoBehaviour {

	Vector2 active_dir = Vector2.zero;
	public GameObject FireBall;

	Transform bike;

	enum state
	{
		WAITING,
		SWINGING,
		TUMBLING,
		JUMPING
	}

	state State = state.WAITING;
	float currAngle = 0;

	float rotationAmount = 0;

	Vector3 Offset = Vector3.zero;

	Vector3 AimOfWeapon = Vector3.zero;

	// Use this for initialization
	void Start () {
		bike = GameObject.FindGameObjectWithTag("Bike").transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {

		switch (State)
		{
			case state.WAITING:
			{
				AimOfWeapon = bike.position + bike.forward * 10;

				Offset = Vector3.up * 0.5f;

				if (active_dir.x != 0)
				{
					AimOfWeapon = bike.position - bike.forward * 10 + bike.right * active_dir.x * 10;
				}
				else if (active_dir.y < 0)
				{
					AimOfWeapon = bike.position - bike.forward * 10 + bike.up * 10;
				}
				else if (active_dir.y > 0)
				{
					AimOfWeapon = bike.position - bike.up * 10;
					Offset = Vector3.up * 3;
				}

				Quaternion goalRotation = Quaternion.LookRotation(AimOfWeapon - bike.position);

				transform.rotation = Quaternion.Slerp(transform.rotation, goalRotation, Time.deltaTime * 20);


				if (Input.GetAxis("Horizontal_RightStick") > 0.4f)
				{
					active_dir = new Vector2(1, 0);

				}
				else if (Input.GetAxis("Horizontal_RightStick") < -0.4f)
				{
					active_dir = new Vector2(-1, 0);
				}
				else if (Input.GetAxis("Vertical_RightStick") < -0.4f)
				{
					active_dir = new Vector2(0, -1);

				}
				else if (Input.GetAxis("Vertical_RightStick") > 0.4f)
				{
					active_dir = new Vector2(0, 1);
				}
				else
				{
					if (active_dir.x != 0)
					{
						State = state.SWINGING;
					}
					else if (active_dir.y < 0)
					{
						State = state.TUMBLING;
					}
					else if (active_dir.y > 0)
					{
						bike.GetComponent<ATV_Controls>().Jump();
						State = state.JUMPING;
					}
				}
				Debug.Log(active_dir);
			} break;

			case state.SWINGING:
			{
				rotationAmount += Time.deltaTime * 1200;
				transform.RotateAround(transform.position, bike.up, Time.deltaTime * active_dir.x * -1200);

				if (rotationAmount > 450)
				{
					active_dir = Vector2.zero;
					rotationAmount = 0;
					State = state.WAITING;
				}
			} break;

			case state.TUMBLING:
			{
				rotationAmount += Time.deltaTime * 1200;
				transform.RotateAround(transform.position, bike.right, Time.deltaTime * 1200);

				if (rotationAmount > 120)
				{
					if (bike.GetComponent<ATV_Controls>().GetGrounded())
					{
					active_dir = Vector2.zero;
					rotationAmount = 0;
					State = state.WAITING;
					}
					else
					{
						rotationAmount -= 360;
					}

					GameObject clone = (GameObject)Instantiate(FireBall, transform.position + Vector3.up, transform.rotation);
					clone.GetComponent<Rigidbody>().velocity = new Vector3(transform.forward.x, 0, transform.forward.z) * 80;

				}
			} break;

			case state.JUMPING:
			{
				rotationAmount += Time.deltaTime * 20;
				Offset -= Vector3.up * Time.deltaTime * 20;

				if (rotationAmount > 3)
				{
					
					active_dir = Vector2.zero;
					rotationAmount = 0;
					State = state.WAITING;


				}
			} break;
		}


		Vector3 dist = ((bike.position + Offset) - transform.position);
		transform.position += dist * Time.deltaTime * 30.0f;



	}
}
