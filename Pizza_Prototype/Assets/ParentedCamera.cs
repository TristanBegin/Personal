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

    float distance = 10;
    float height = 5;

    float angle = 90;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
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
        transform.localPosition = Quaternion.Euler(0, angle, 0) * transform.localPosition;
        transform.LookAt(target);

    }
}
