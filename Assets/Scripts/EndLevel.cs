using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    MenuManager menuManager;
    Player aController;

	//Use this for initialization
	void Start()
    {
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        aController = GameObject.Find("Player").GetComponent<Player>();
    }
	
	//Update is called once per frame
	void Update()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (aController.BeerTipAmount <= 0)
                aController.BeerTipAmount = 0;

            //Inputing default values for now
            menuManager.ShowEndLevelScreen(aController.BeersServed, aController.BeerTipAmount, 1.00, 0);
        }
    }
}
