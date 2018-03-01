using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public enum Screen { MAIN_MENU = 0, GAME = 1 };

    //Menu Groups
    private CanvasGroup StartMenu;
    private CanvasGroup CreditsMenu;
    [SerializeField] private Screen currentScreen;


    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);    
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
        CreditsMenu.alpha = 1;
        CreditsMenu.interactable = true;
        CreditsMenu.blocksRaycasts = true;

        //Turn off All Other Groups
        StartMenu.alpha = 0;
        StartMenu.interactable = false;
        StartMenu.blocksRaycasts = false;
    }

    public void ShowStart()
    {
        //Turn on StartMenu
        StartMenu.alpha = 1;
        StartMenu.interactable = true;
        StartMenu.blocksRaycasts = true;

        //Turn off All other Groups
        CreditsMenu.alpha = 0;
        CreditsMenu.interactable = false;
        CreditsMenu.blocksRaycasts = false;
    }

    public void ShowControls()
    {
        //Turn on ControlMenu

        //Turn off All other Groups
        CreditsMenu.alpha = 0;
        CreditsMenu.interactable = false;
        CreditsMenu.blocksRaycasts = false;
        StartMenu.alpha = 0;
        StartMenu.interactable = false;
        StartMenu.blocksRaycasts = false;
    }

    public void GoToScene(int screenEnum)
    {
        
        switch(screenEnum)
        {
            case (int)Screen.MAIN_MENU:
                SceneManager.LoadScene("MainMenu");
                Destroy(this);
                break;

            case (int)Screen.GAME:
                SceneManager.LoadScene("Game");
                break;
        }
    }
}
