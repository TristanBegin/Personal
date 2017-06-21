using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {
    public LayerMask TriggeringLayers;
    public bool Active = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void OnTriggerStay (Collider hit)
    {
        if (TriggeringLayers == (TriggeringLayers | (1 << hit.gameObject.layer)))
        {
            Active = true;
        }

    }

    void OnTriggerExit(Collider hit)
    {
        if (TriggeringLayers == (TriggeringLayers | (1 << hit.gameObject.layer)))
        {
            Active = false;
        }
    }
}
