using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    public enum Screen { MAIN_MENU = 0, GAME = 1, PAUSE = 2 };

    //Menu Groups
    private CanvasGroup StartMenu;
    private CanvasGroup ControlsMenu;
    private CanvasGroup CreditsMenu;
    private CanvasGroup PauseMenu;
    private CanvasGroup GameUI;
    private CanvasGroup DebugUI;
    private CanvasGroup EndLevelMenu;

    //End Level Screen UI Elements
    private Text patronServed;
    private Text tipsEarned;
    private Text timeCompleted;
    //private Button tryAgainBtn;
    private Button continueBtn;

    //Patron Target Prefabs
    private GameObject patronTarget;
    [SerializeField] private GameObject joelPatron;
    [SerializeField] private GameObject joshPatron;
    [SerializeField] private GameObject johnPatron;
    [SerializeField] private GameObject unknownPatron;

    //Menu Manager variables
    [SerializeField] private Screen currentScreen;
    private bool DebugOn = false;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);    
    }

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Use this for initialization
    void Start()
    {
        StartMenu = GameObject.Find("StartMenu").GetComponent<CanvasGroup>();
        ControlsMenu = GameObject.Find("ControlsMenu").GetComponent<CanvasGroup>();
        CreditsMenu = GameObject.Find("CreditsMenu").GetComponent<CanvasGroup>();

    }
	
	//Update is called once per frame
	void Update()
    {
		
	}

    public void ShowCredits()
    {
        //Turn on Credits Menu
        TurnOn(CreditsMenu);

        //Turn off All Other Groups
        TurnOff(StartMenu);
        TurnOff(ControlsMenu);
    }

    public void ShowStart()
    {
        //Turn on StartMenu
        TurnOn(StartMenu);

        //Turn off All other Groups
        TurnOff(ControlsMenu);
        TurnOff(CreditsMenu);
    }

    public void ShowControls()
    {
        //Turn on ControlMenu
        TurnOn(ControlsMenu);

        //Turn off All other Groups
        TurnOff(CreditsMenu);
        TurnOff(StartMenu);
    }

    public void PauseGame()
    {
        //Freeze Time in Game
        Time.timeScale = 0;

        //Turn on Pause Menu
        TurnOn(PauseMenu);

        //set current Screen to Pause
        currentScreen = Screen.PAUSE;
    }

    public void ResumeGame()
    {
        //Turn Pause Screen Off
        TurnOff(PauseMenu);

        //Set current screen to Game
        currentScreen = Screen.GAME;

        //Unfreeze Time
        Time.timeScale = 1;
    }

    /// <summary>
    /// Shows the end level screen for the current level. Must be given the proper values to display to player in the menu
    /// </summary>
    /// <param name="patronAmount">Amount of patrons player served</param>
    /// <param name="tipAmount">Amount of tips earned by player</param>
    /// <param name="time">Amount of time it took player to reach end of level</param>
    /// <param name="nextLvl">int value for build order of the scene that contains the next level</param>
    public void ShowEndLevelScreen(int patronAmount, double tipAmount, double time, int nextLvl)
    {
        patronServed.text = patronAmount + " Patrons Served";
        tipsEarned.text = "$ " + tipAmount + " Earned";
        timeCompleted.text = time.ToString();

        continueBtn.onClick.AddListener(delegate { GoToScene(nextLvl); });

        TurnOn(EndLevelMenu);
    }

    public void SwitchPatronTarget(Patron.Patrons newTarget)
    {
        //Delete Old Patron Target
        if (patronTarget != null)
        {
            Destroy(patronTarget);
        }

        switch (newTarget)
        {
            case Patron.Patrons.JOEL:
                patronTarget = Instantiate(joelPatron,GameUI.transform);
                break;

            case Patron.Patrons.JOHN:
                patronTarget = Instantiate(johnPatron, GameUI.transform);
                break;

            case Patron.Patrons.JOSH:
                patronTarget = Instantiate(joshPatron, GameUI.transform);
                break;

            case Patron.Patrons.UNKNOWN:
                patronTarget = Instantiate(unknownPatron, GameUI.transform);
                break;
        }
    }

    public void ToggleDebug()
    {
        if(DebugOn)
        {
            TurnOff(DebugUI);
        }
        else
        {
            TurnOn(DebugUI);
        }
        DebugOn = !DebugOn; 
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void TurnOn(CanvasGroup screen)
    {
        screen.alpha = 1;
        screen.interactable = true;
        screen.blocksRaycasts = true;
    }

    public void TurnOff(CanvasGroup screen)
    {
        screen.alpha = 0;
        screen.interactable = false;
        screen.blocksRaycasts = false;
    }

    public void GoToScene(int screenEnum)
    {
        switch(screenEnum)
        {
            case (int)Screen.MAIN_MENU:
                SceneManager.LoadScene("MainMenu");
                break;

            case (int)Screen.GAME:
                SceneManager.LoadScene("Game");
                break;
        }
    }

    public Screen GetCurrentScreen()
    {
        return currentScreen;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                currentScreen = Screen.MAIN_MENU;
                //Destroy(this);
                break;

            case "Game":
                //Get Game UI
                GameUI = GameObject.Find("GameUI").GetComponent<CanvasGroup>();
                //Set First Patron Target to serve
                Patron[] patrons = GameObject.Find("Patrons").GetComponentsInChildren<Patron>();
                foreach(Patron patron in patrons)
                {
                    if(patron.PatronNum == 1)
                    {
                        SwitchPatronTarget(patron.PatronId);
                        break;
                    }
                }

                //Configure Pause Menus
                PauseMenu = GameObject.Find("PauseMenu").GetComponent<CanvasGroup>();
                GameObject.Find("PauseBtn").GetComponent<Button>().onClick.AddListener(PauseGame);
                GameObject.Find("ResumeBtn").GetComponent<Button>().onClick.AddListener(ResumeGame);
                GameObject.Find("RestartBtn").GetComponent<Button>().onClick.AddListener(delegate { Time.timeScale = 1; GoToScene(0); });
                GameObject.Find("DebugBtn").GetComponent<Button>().onClick.AddListener(ToggleDebug);
                GameObject.Find("Exit").GetComponent<Button>().onClick.AddListener(Exit);

                GameUI = GameObject.Find("GameUI").GetComponent<CanvasGroup>();
                EndLevelMenu = GameObject.Find("EndLevelScreen").GetComponent<CanvasGroup>();

                //Find End Level UI elements
                patronServed = GameObject.Find("Patron Score").GetComponent<Text>();
                tipsEarned = GameObject.Find("Tip Score").GetComponent<Text>();
                timeCompleted = GameObject.Find("timeCompleted").GetComponent<Text>();
                GameObject.Find("TryAgainBtn").GetComponent<Button>().onClick.AddListener(delegate { Time.timeScale = 1; GoToScene(0); });
                continueBtn = GameObject.Find("ContinueBtn").GetComponent<Button>();
                break;
        }
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
    }
}
