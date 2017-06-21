using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BecomePokable : MonoBehaviour {
    public Material newMaterial;
	// Use this for initialization
	void Start () {
        GetComponent<Pokable>().enabled = false;
	}
	
	// Update is called once per frame
	void OnTriggerExit (Collider hit) {
        if (hit.gameObject.GetComponent<Poker>())
            GetComponent<Pokable>().enabled = true;

        GetComponent<Renderer>().material = newMaterial;
	}
}
