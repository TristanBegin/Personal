using UnityEngine;
using System.Collections;

public class GrindRailNode : MonoBehaviour {

    public Transform NextNode;
    public Transform PreviousNode;
    public LayerMask grindMask;

	// Use this for initialization
	void Start () {
	
	}

    void Update()
    {
        if (NextNode != null)
        {
            RaycastHit hit;
            Vector3 vecToNext = NextNode.position - transform.position;
            if (Physics.SphereCast(transform.position, GetComponent<Collider>().bounds.extents.x, vecToNext.normalized, out hit, vecToNext.magnitude - GetComponent<Collider>().bounds.extents.x - 10, grindMask))
            {
                if (hit.transform.tag == "Player")
                {
                    if (NextNode != null)
                    {
                        //hit.transform.GetComponent<HoverBoard>().GrindToward(NextNode);
                    }
                    else
                    {
                        //hit.transform.GetComponent<HoverBoard>().EndGrind();
                    }
                }
            }

            Vector3[] positions = { transform.position, NextNode.position };
            GetComponent<LineRenderer>().SetPositions(positions);
        }
    }
	
	// Update is called once per frame
	void OnTriggerEnter (Collider hit)
    {
	    if (hit.transform.tag == "Player")
        {
            if (NextNode != null)
            {
                //hit.GetComponent<HoverBoard>().GrindToward(NextNode);
            }
            else
            {
                //hit.GetComponent<HoverBoard>().EndGrind();
            }
        }
	}
}
