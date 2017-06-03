using UnityEngine;
using System.Collections;

public class Hook : MonoBehaviour {

    public GameObject Shooter;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter (Collider hit)
    {
        Shooter.SendMessage("HookCaught", hit.gameObject);
	}
}
