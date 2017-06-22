using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poker : MonoBehaviour {

    public Pokable PokedObject = null;

    public bool TryingToPoke = true;

    public bool HasObject { get { return PokedObject != null; } }

    public Transform mainTransform = null;

	// Update is called once per frame
	void OnTriggerEnter (Collider hit)
    {
        if (transform.parent == null)
            Debug.Log("" + (hit.GetComponent<Pokable>() != null) + ", " + (PokedObject == null) + ", " + TryingToPoke);
        if (hit.GetComponent<Pokable>() != null && PokedObject == null && TryingToPoke)
        {
            hit.GetComponent<Pokable>().GotPoked(this);
        }
	}

    public void ForcePoke(Pokable pokable)
    {
        pokable.GotPoked(this);
    }
    

    public void PokeAccepted(Pokable pokable)
    {
        PokedObject = pokable;
        //pokable.transform.parent = transform;
        //pokable.transform.localPosition = Vector3.zero;
    }

    public void LostObject()
    {
        PokedObject = null;
    }

    public void Detach()
    {
        if (PokedObject != null)
            PokedObject.Detach();
    }
}
