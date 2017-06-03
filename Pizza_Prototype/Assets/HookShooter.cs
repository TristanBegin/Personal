using UnityEngine;
using System.Collections;

public class HookShooter : MonoBehaviour {

    public GameObject HookPrefab;
    public Transform Camera;
    public LayerMask Hookables;

    GameObject HookedObject;

    LineRenderer myLine;

    GameObject Hook;
    float hookLength;

    float hookTime = 0;

	// Use this for initialization
	void Start () {
        myLine = GetComponentInChildren<LineRenderer>();    
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetButtonDown("RB") || Input.GetButtonDown("LB"))
        {
            Destroy(Hook);

            if (HookedObject == null)
            {
                Hook = Instantiate(HookPrefab);
                Hook.transform.position = transform.position;
                Hook.GetComponent<Hook>().Shooter = gameObject;
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
                        
            if (HookedObject != null)
            {
                Hook.transform.position = HookedObject.transform.position;
                Hook.GetComponent<Rigidbody>().velocity = Vector3.zero;

                Debug.Log(hookLength);
                hookLength += -Input.GetAxis("Triggers") * Time.deltaTime * 35;

                hookLength = Mathf.Clamp(hookLength, 3, 100);
            }
            else if(hookTime > 1)
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

    public float GetHookLength()
    {
        return hookLength;
    }

    void HookCaught(GameObject hit)
    {
        if (hit.tag == "Enemy" && HookedObject == null)
        {
            Debug.Log("Hook");
            HookedObject = hit;
            hookLength = (hit.transform.position - transform.position).magnitude;
        }
    }
}
