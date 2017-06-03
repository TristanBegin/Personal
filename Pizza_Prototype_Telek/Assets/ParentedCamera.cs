using UnityEngine;
using System.Collections;

public class ParentedCamera : MonoBehaviour {

    public Transform target;

    enum CameraMode
    {
        BACK,
        RIGHT,
        FRONT,
        LEFT
    }

    CameraMode mode = CameraMode.BACK;

    float distance = 15;
    float height = 5;

    float angle = 90;

	// Use this for initialization
	void Start ()
    {
	
	}

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("RB"))
        {
            mode++;
        }
        else if (Input.GetButtonDown("LB"))
        {
            mode--;
        }

        if (mode > CameraMode.LEFT) mode = CameraMode.BACK;
        if (mode < CameraMode.BACK) mode = CameraMode.LEFT;

        switch (mode)
        {
            case CameraMode.BACK:
                angle = 0;
                break;

            case CameraMode.RIGHT:
                angle = 270;
                break;

            case CameraMode.FRONT:
                angle = 180;
                break;

            case CameraMode.LEFT:
                angle = 90;
                break;
        }

        transform.localPosition = new Vector3(0, height, -distance);

        GameObject hookedObject = null;//GetComponentInParent<HookShooter>().GetHookedObject();
        if (hookedObject != null)
        {
            Vector3 lookVector = hookedObject.transform.position - transform.parent.position;
            Vector3 mod_lookVector = Vector3.ProjectOnPlane(lookVector.normalized, transform.parent.up);
            //Vector3 toTargetVector = target.position - transform.position;
            //Vector3 mod_toTargetVector = Vector3.ProjectOnPlane(toTargetVector.normalized, transform.parent.up);
            //angle = Vector3.Angle(mod_toTargetVector, mod_lookVector);
            //
            //transform.localPosition = Quaternion.* transform.localPosition;
            //
            //if (Vector3.Dot(Vector3.ProjectOnPlane(transform.localPosition, Vector3.up), mod_toTargetVector) > 0)
            //{
            //    transform.localPosition = new Vector3 (-transform.localPosition.x, transform.localPosition.y, -transform.position.z);
            //}
            transform.position = transform.parent.position + new Vector3(-mod_lookVector.x * distance, height, -mod_lookVector.z * distance);
        }
        else
        {
            //transform.localPosition = Quaternion.Euler(0, angle, 0) * transform.localPosition;
        }

        transform.LookAt(target);

    }
}
