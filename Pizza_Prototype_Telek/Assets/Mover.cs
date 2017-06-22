using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    Pokable pokable;
	// Use this for initialization
	void Start () {
        pokable = GetComponent<Pokable>();
	}
	
	// Update is called once per frame
	void Update () {
		if (pokable.IsPoked && pokable.myPoker.mainTransform != null)
        {
            pokable.myPoker.mainTransform.position -= transform.forward * Time.deltaTime * 6;
        }
	}
}
