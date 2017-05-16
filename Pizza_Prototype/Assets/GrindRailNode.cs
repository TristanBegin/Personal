using UnityEngine;
using System.Collections;

public class GrindRailNode : MonoBehaviour {

    public Transform NextNode;
    public Transform PreviousNode;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider hit)
    {
	    if (hit.transform.tag == "Player")
        {
            if (NextNode != null)
            {
                hit.GetComponent<HoverBoard>().GrindToward(NextNode);
            }
            else
            {
                hit.GetComponent<HoverBoard>().EndGrind();
            }
        }
	}
}
