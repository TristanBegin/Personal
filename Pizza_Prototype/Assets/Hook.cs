using UnityEngine;
using System.Collections;

public class Hook : MonoBehaviour {

    public GameObject Shooter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D hit)
    {
        Shooter.SendMessage("HookCaught", hit.gameObject);
	}
}
