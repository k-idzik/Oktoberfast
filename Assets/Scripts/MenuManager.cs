using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    public enum Screen { MAIN_MENU = 0, GAME = 1, PAUSE = 2 };

    //Menu Groups
    private CanvasGroup StartMenu;
    private CanvasGroup CreditsMenu;
    [SerializeField] private CanvasGroup PauseMenu;
    private CanvasGroup GameUI;
    private CanvasGroup DebugUI;

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

    // Use this for initialization
    void Start () {
        StartMenu = GameObject.Find("StartMenu").GetComponent<CanvasGroup>();
        CreditsMenu = GameObject.Find("CreditsMenu").GetComponent<CanvasGroup>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowCredits()
    {
        //Turn on Credits Menu
        TurnOn(CreditsMenu);

        //Turn off All Other Groups
        TurnOff(StartMenu);
    }

    public void ShowStart()
    {
        //Turn on StartMenu
        TurnOn(StartMenu);

        //Turn off All other Groups
        TurnOff(CreditsMenu);
    }

    public void ShowControls()
    {
        //Turn on ControlMenu

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
                //Configure Pause Menus
                PauseMenu = GameObject.Find("PauseMenu").GetComponent<CanvasGroup>();
                GameObject.Find("PauseBtn").GetComponent<Button>().onClick.AddListener(PauseGame);
                GameObject.Find("ResumeBtn").GetComponent<Button>().onClick.AddListener(ResumeGame);
                GameObject.Find("RestartBtn").GetComponent<Button>().onClick.AddListener(delegate { Time.timeScale = 1; GoToScene(0); });
                GameObject.Find("DebugBtn").GetComponent<Button>().onClick.AddListener(ToggleDebug);
                GameObject.Find("Exit").GetComponent<Button>().onClick.AddListener(Exit);

                DebugUI = GameObject.Find("DebugGroup").GetComponent<CanvasGroup>();
                GameUI = GameObject.Find("GameUI").GetComponent<CanvasGroup>();
                break;

        }
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
    }
}
