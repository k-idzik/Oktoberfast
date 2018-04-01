using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private MeshRenderer renderer;
    private Collider thisCollider;
    private int rendererTimer = 0; //Timer for mesh blink

	//Use this for initialization
	void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        thisCollider = GetComponent<Collider>();
	}
	
	//Update is called once per frame
	void Update()
    {

	}

    //Colliding with table
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            thisCollider.isTrigger = true;
        }
    }

    private void OnTriggerStay(Collider coll)
    {
        if (rendererTimer < 10)
            renderer.enabled = false;
        else
            renderer.enabled = true;

        rendererTimer++;

        if (rendererTimer > 20)
            rendererTimer = 0;
    }

    //Leaving collision with table
    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            thisCollider.isTrigger = false; //Make this a collider again
            rendererTimer = 0; //Reset the timer
            renderer.enabled = true; //Turn the renderer back on
        }
    }
}
