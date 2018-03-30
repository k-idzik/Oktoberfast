using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    //Game Manager attributes
    private Patron[] patrons;
    private Patron currentPatron;               //Current Patron target that Player must serve
    private int gameTimer;                      //Time Left on Game Timer
    [SerializeField] private int patronTime;    //Time player has to serve patron
    private Coroutine timer; //Coroutine that holds Patron Timer
    private float startTime; //Time when game started
    private MenuManager menuManager;
    private Player player; 

    public Patron[] Patrons
    {
        get { return patrons; }
    }

    public Patron CurrentPatron { get { return currentPatron; } }
    public int GameTimer { get { return gameTimer; } }
    public int PatronTime { get { return patronTime; } }

    //Instance management to avoid duplicate singletons
    //https://answers.unity.com/questions/408518/dontdestroyonload-duplicate-object-in-a-singleton.html
    public static GameManager gameMan;
    private void Awake()
    {
        //Prevent duplicates
        if (!gameMan)
            gameMan = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        if (menuManager == null)
            menuManager = MenuManager.Instance;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        //Can't just get it in the player via instance after restart
        //So we do it here
        //Why
        player.GManager = this;
        player.MenuMan = menuManager;

        startTime = Time.unscaledTime;
        //Get All Patrons in level
        patrons = GameObject.Find("Patrons").GetComponentsInChildren<Patron>();

        //Get 1st Patron to serve
        int patronIndex = Random.Range(0, patrons.Length - 1);
        currentPatron = patrons[patronIndex];
        currentPatron.GetComponent<SpriteRenderer>().enabled = true; //Use GetComponent for starting the game because the Patron's Start Method hasn't run yet.
        currentPatron.CurrentPatron = true;

        //Set Patron Target
        menuManager.SwitchPatronTarget(currentPatron.PatronId);

        //Reset the game
        ResetGame();

        //Start Patron Timer
        menuManager.UpdateTimer();
        timer = StartCoroutine(PatronTimer());
    }

    private void ResetGame()
    {
        ResetPatronTimer(); //Reset the timer so we can actually replay
        Time.timeScale = 1; //And let us move
    }

    public void ResetPatronTimer()
    {
        gameTimer = 0;
        menuManager.UpdateTimer();
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

    void GameOver()
    {
        Time.timeScale = 0;
        menuManager.ShowEndLevelScreen(player.BeersServed, player.BeerTipAmount, Time.unscaledTime - startTime, 0);
    }

    private IEnumerator PatronTimer()
    {
        while(gameTimer < PatronTime) //While GameTimer Has Not exceed Patron time
        {
            yield return new WaitForSeconds(1);
            gameTimer++;

            //Check the name of the current scene in case we leave the game via pause
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu" || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Controls")
            {
                yield break;
            }

            menuManager.UpdateTimer();
        }

        GameOver();
    }
}
