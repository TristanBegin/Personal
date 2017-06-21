using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetroyOnHitGround : MonoBehaviour {
    public LayerMask hittableLayers;
	void OnTriggerEnter (Collider hit)
    {
        if (hittableLayers == (hittableLayers | (1 << hit.gameObject.layer)))
            Destroy(gameObject);
	}
}
