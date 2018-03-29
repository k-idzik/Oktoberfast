using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    //Game Manager attributes
    private Patron[] patrons;
    private Patron currentPatron; //Current Patron target that Player must serve

    public Patron[] Patrons
    {
        get { return patrons; }
    }

    public Patron CurrentPatron { get { return currentPatron; } }

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void StartGame()
    {
        //Get All Patrons in level
        patrons = GameObject.Find("Patrons").GetComponentsInChildren<Patron>();

        //Get 1st Patron to serve
        int patronIndex = Random.Range(0, patrons.Length - 1);
        currentPatron = patrons[patronIndex];
        currentPatron.GetComponent<SpriteRenderer>().enabled = true; //Use GetComponent for starting the game because the Patron's Start Method hasn't run yet.
        currentPatron.CurrentPatron = true;

        //Set Patron Target
        MenuManager.Instance.SwitchPatronTarget(currentPatron.PatronId);
    }

    private void ResetGame()
    {
        
    }

    public Patron NewPatronTarget()
    {
        bool newPatron = false; //indicates whether new patron has been selected yet
        while(!newPatron) //While a new Patron has not been Selected
        {
            int patronIndex = Random.Range(0, patrons.Length - 1);

            Patron potentialPatron = patrons[patronIndex];

            //Check to see if this patron was same patron that was just served
            if(potentialPatron != currentPatron)
            {
                //Turn off Previous Patron
                currentPatron.Renderer.enabled = false;
                currentPatron.CurrentPatron = false;

                currentPatron = potentialPatron;
                currentPatron.Renderer.enabled = true;
                CurrentPatron.CurrentPatron = true;

                newPatron = true;
            }
        }
        return currentPatron;
    }
}
