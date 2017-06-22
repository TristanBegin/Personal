using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    Pokable pokable;
	// Use this for initialization
	void Start () {
        pokable = GetComponent<Pokable>();
	}
	
	// Update is called once per frame
	void Update () {
		if (pokable.IsPoked && pokable.myPoker.mainTransform != null)
        {
            pokable.myPoker.mainTransform.RotateAround(transform.position, transform.forward, Time.deltaTime * 30);
        }
	}
}
