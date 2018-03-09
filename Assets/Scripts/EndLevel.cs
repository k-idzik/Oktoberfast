using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour {

    MenuManager menuManager;

	// Use this for initialization
	void Start () {
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Inputing default values for now
            menuManager.ShowEndLevelScreen(0, 0, 1.00, 0);
        }

    }
}
