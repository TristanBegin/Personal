using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokable : MonoBehaviour {

    Poker previousPoker = null;
    [HideInInspector] public Poker myPoker = null;

    float previousPokerCooldown = 0;

    public bool IsPoked { get { return myPoker != null; } }

	// Use this for initialization
	public void GotPoked (Poker poker)
    {
        if (poker == previousPoker)
            return;
        
        previousPoker = myPoker;
        myPoker = poker;

        previousPokerCooldown = 1;

        myPoker.PokeAccepted(this);
        if (previousPoker != null)
            previousPoker.LostObject();

        transform.parent = null;

    }

    public void Detach()
    {
        if (myPoker == null)
            return;

        myPoker.LostObject();
        previousPoker = myPoker;
        myPoker = null;
    }

    // Update is called once per frame
    void Update () {
        if (myPoker != null)
        {
            transform.position = myPoker.transform.position;
            transform.forward = myPoker.transform.forward;
        }

        previousPokerCooldown -= Time.deltaTime;
        if (previousPokerCooldown < 0)
            previousPoker = null;
	}
}
