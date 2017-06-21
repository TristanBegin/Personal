using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	public Transform enemy;

    public float desiredDistance;

	Vector3 ForwardMovement;
	Vector3 UpMovement;
	Vector3 RightMovement;

	Vector3 UpBias;
	Vector3 RightBias;

    Vector3 target_lazyPos;

    Vector3 saved_vecToTarget;

    Player playerScript;

	float desiredYOffset = 2;

	float RightSpeed = 0;

    Transform playerBoard;

    Transform AttachedPole;

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


        Vector3 vecToTarget = target_lazyPos - transform.position;
        
        if (saved_vecToTarget == Vector3.zero)
            saved_vecToTarget = new Vector3(vecToTarget.x, -3, vecToTarget.z).normalized;
        
        transform.position = target_lazyPos - saved_vecToTarget * desiredDistance;
        if (Input.GetButton("LB") == false)
        {
            saved_vecToTarget = Quaternion.Euler(new Vector3(0, Input.GetAxis("Horizontal_RS"), 0) * Time.deltaTime * 200) * saved_vecToTarget;
            saved_vecToTarget += Input.GetAxis("Vertical_RS") * Time.deltaTime * Vector3.Cross(vecToTarget.normalized, transform.right);
            saved_vecToTarget.Normalize();
        }

        //if (Catcher.Rolling)
        //{
        //    saved_vecToTarget += ((target.forward * desiredDistance + target.up * -3).normalized - saved_vecToTarget) * Time.deltaTime * 1.5f;
        //}

        transform.LookAt(target_lazyPos);



        float currentYOffset = transform.position.y - target_lazyPos.y;


        //Vector3 vecToEnemy = Vector3.ProjectOnPlane(enemy.transform.position - target.transform.position, transform.up);
        //Vector3 otherVec = Vector3.Cross(vecToEnemy, enemy.up);
        //RightBias = Vector3.Cross(vecToTarget.normalized, transform.up) * Vector3.Dot(-vecToTarget.normalized, -otherVec.normalized) * 400;
        UpBias = Vector3.Cross(vecToTarget.normalized, transform.right) * (desiredYOffset - currentYOffset) * 5;


        

        GetComponent<Rigidbody>().velocity = UpMovement + UpBias;

        if (enemy != null)
        {
            Vector3 lookVector = enemy.transform.position - target.position;
            Vector3 mod_lookVector = Vector3.ProjectOnPlane(lookVector.normalized, target.up);
            transform.position = target.position + new Vector3(-mod_lookVector.x * desiredDistance, transform.position.y - target.position.y, -mod_lookVector.z * desiredDistance);
        }
    }

	void UpdateLazyPos()
	{
		//Vector3 vecToTarget = target.transform.position - target_lazyPos;
		//Vector3 vecToTarget_noY = new Vector3 (vecToTarget.x, 0, vecToTarget.z);
		//target_lazyPos += vecToTarget_noY * Time.deltaTime * 6;
        //
		//target_lazyPos.y += vecToTarget.y * Time.deltaTime * 2;

        target_lazyPos = target.position;

	}
}
