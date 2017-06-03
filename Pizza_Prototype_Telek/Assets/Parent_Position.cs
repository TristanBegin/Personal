using UnityEngine;
using System.Collections;

public class Parent_Position : MonoBehaviour {

	public Transform pseudoparent;
	public Vector3 offset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = pseudoparent.position + offset;
	}
}
