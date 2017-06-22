using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour {

    Pokable pokable;
    Transform attachment = null;
    // Use this for initialization
    void Start()
    {
        pokable = GetComponent<Pokable>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (attachment != null)
        //{
        //    Vector3[] positions = { transform.position, attachment.position };
        //    GetComponent<LineRenderer>().SetPositions(positions);
        //}
    }

    public void Caught(Transform other)
    {
        attachment = other;
    }

    void OnTriggerEnter(Collider hit)
    {
        hit.SendMessage("Attract", this);
    }
}
