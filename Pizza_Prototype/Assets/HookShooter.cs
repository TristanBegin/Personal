using UnityEngine;
using System.Collections;

public class HookShooter : MonoBehaviour {

    public GameObject HookPrefab;
    public Transform Camera;
    public LayerMask Hookables;

    GameObject HookedObject;

    LineRenderer myLine;

    GameObject Hook;

    float hookTime = 0;

	// Use this for initialization
	void Start () {
        myLine = GetComponentInChildren<LineRenderer>();    
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetButtonDown("X_Button"))
        {
            Destroy(Hook);

            if (HookedObject == null)
            {
                Hook = Instantiate(HookPrefab);
                Hook.transform.position = transform.position;
                Hook.GetComponent<Rigidbody>().velocity = Vector3.ProjectOnPlane(Camera.forward, transform.up) * 200;
            }
            else
            {
                HookedObject = null;
            }
        }

        if (Hook != null)
        {
            Vector3[] positions = { transform.position, Hook.transform.position };
            myLine.SetPositions(positions);

            hookTime += Time.deltaTime;

            if (hookTime > 1)
            {
                Destroy(Hook);
                Hook = null;
                hookTime = 0;
                Vector3[] positionsNull = { Vector3.zero, Vector3.zero };
                myLine.SetPositions(positionsNull);
            }

            
        }
	}

    public GameObject GetHookedObject()
    {
        return HookedObject;
    }

    void HookCaught(GameObject hit)
    {
        if (Hookables == (Hookables | (1 << hit.layer)))
        {
            HookedObject = hit;
        }
    }
}
