using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    private MeshRenderer renderer;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Colliding with table
    private void OnTriggerEnter(Collider coll)
    {

    }

    //Leaving collision with table
    private void OnTriggerExit(Collider coll)
    {
       
    }


    
}
