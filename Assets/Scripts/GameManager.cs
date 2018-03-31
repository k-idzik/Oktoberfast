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
    private SoundManager soundMan;
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
        if (soundMan == null)
            soundMan = SoundManager.Instance;

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
                #region Delivery Sounds
                int soundIndex; //Determine what sound to play

                //Determine which sound to play upon delivery
                if (player.MaxBeerTilt < 100) //Bad delivery
                {
                    if (currentPatron.name.Contains("Josh"))
                    {
                        soundIndex = Random.Range(0, 10); //10 negative Josh sounds

                        switch(soundIndex)
                        {
                            case 0:
                                soundMan.PlaySfxAt("Josh_BringMeABeer0", player.transform.position, 100);
                                break;
                            case 1:
                                soundMan.PlaySfxAt("Josh_BringMeABeer1", player.transform.position, 100);
                                break;
                            case 2:
                                soundMan.PlaySfxAt("Josh_BringMeMyBeer0", player.transform.position, 100);
                                break;
                            case 3:
                                soundMan.PlaySfxAt("Josh_BringMeMyBeer1", player.transform.position, 100);
                                break;
                            case 4:
                                soundMan.PlaySfxAt("Josh_BringMeMyBeerWench", player.transform.position, 100);
                                break;
                            case 5:
                                soundMan.PlaySfxAt("Josh_CanYouNot", player.transform.position, 100);
                                break;
                            case 6:
                                soundMan.PlaySfxAt("Josh_GetBetter", player.transform.position, 100);
                                break;
                            case 7:
                                soundMan.PlaySfxAt("Josh_OhNoMyBeersOnTheGroundThatsOkay", player.transform.position, 100);
                                break;
                            case 8:
                                soundMan.PlaySfxAt("Josh_StopThisIDontLikeThis", player.transform.position, 100);
                                break;
                            case 9:
                                soundMan.PlaySfxAt("Josh_WhyMustYouSpill", player.transform.position, 100);
                                break;
                            default:
                                soundMan.PlaySfxAt("Josh_OhNoMyBeersOnTheGroundThatsOkay", player.transform.position, 100);
                                break;
                        }
                    }
                    else if (currentPatron.name.Contains("John"))
                    {
                        soundIndex = Random.Range(0, 8); //8 negative John sounds

                        switch (soundIndex)
                        {
                            case 0:
                                soundMan.PlaySfxAt("John_HeyWatchWhereYoureSpillingThat", player.transform.position, 100);
                                break;
                            case 1:
                                soundMan.PlaySfxAt("John_ILikeToActuallyDrinkWhatIOrder", player.transform.position, 100);
                                break;
                            case 2:
                                soundMan.PlaySfxAt("John_OhSorryBoutThatDozedOffWithHowLongItTook", player.transform.position, 100);
                                break;
                            case 3:
                                soundMan.PlaySfxAt("John_OiYoureTakingAWhile", player.transform.position, 100);
                                break;
                            case 4:
                                soundMan.PlaySfxAt("John_Snore", player.transform.position, 100);
                                break;
                            case 5:
                                soundMan.PlaySfxAt("John_WhyIEvenComeHere", player.transform.position, 100);
                                break;
                            case 6:
                                soundMan.PlaySfxAt("John_SnoreOhSorryBoutThatDozedOffWithHowLongItTook", player.transform.position, 100);
                                break;
                            case 7:
                                soundMan.PlaySfxAt("John_YeahIdLikeMyBeerFullThanks", player.transform.position, 100);
                                break;
                            default:
                                soundMan.PlaySfxAt("John_ILikeToActuallyDrinkWhatIOrder", player.transform.position, 100);
                                break;
                        }
                    }
                    else if (currentPatron.name.Contains("Joel"))
                    {
                        soundIndex = Random.Range(0, 11); //11 negative Joel sounds

                        switch (soundIndex)
                        {
                            case 0:
                                soundMan.PlaySfxAt("Joel_Alright0", player.transform.position, 100);
                                break;
                            case 1:
                                soundMan.PlaySfxAt("Joel_Alright1", player.transform.position, 100);
                                break;
                            case 2:
                                soundMan.PlaySfxAt("Joel_SoUhhYouDontWantATipThen", player.transform.position, 100);
                                break;
                            case 3:
                                soundMan.PlaySfxAt("Joel_ThatsSadThatsActuallyReallyDisappointing", player.transform.position, 100);
                                break;
                            case 4:
                                soundMan.PlaySfxAt("Joel_ThatWasHorrible", player.transform.position, 100);
                                break;
                            case 5:
                                soundMan.PlaySfxAt("Joel_ThisIsLikeNoBeerLeft", player.transform.position, 100);
                                break;
                            case 6:
                                soundMan.PlaySfxAt("Joel_ThisIsntWhatIOrdered0", player.transform.position, 100);
                                break;
                            case 7:
                                soundMan.PlaySfxAt("Joel_ThisIsntWhatIOrdered1", player.transform.position, 100);
                                break;
                            case 8:
                                soundMan.PlaySfxAt("Joel_Uhh", player.transform.position, 100);
                                break;
                            case 9:
                                soundMan.PlaySfxAt("Joel_YoureKindOfTakingALongTime", player.transform.position, 100);
                                break;
                            case 10:
                                soundMan.PlaySfxAt("Joel_YouSpilledAllMyBeer", player.transform.position, 100);
                                break;
                            default:
                                soundMan.PlaySfxAt("Joel_ThatsSadThatsActuallyReallyDisappointing", player.transform.position, 100);
                                break;
                        }
                    }
                }
                else //Good deliver
                {
                    if (currentPatron.name.Contains("Josh"))
                    {
                        soundIndex = Random.Range(0, 8); //8 positive Josh sounds

                        switch (soundIndex)
                        {
                            case 0:
                                soundMan.PlaySfxAt("Josh_GetMeTwoBeersPlease", player.transform.position, 100);
                                break;
                            case 1:
                                soundMan.PlaySfxAt("Josh_HaveYouEverWonderedWhatSlothsLookLikeWhenTheyreDrunk", player.transform.position, 100);
                                break;
                            case 2:
                                soundMan.PlaySfxAt("Josh_HeyYouBroughtMeMyBeerGoodJob", player.transform.position, 100);
                                break;
                            case 3:
                                soundMan.PlaySfxAt("Josh_Mmm", player.transform.position, 100);
                                break;
                            case 4:
                                soundMan.PlaySfxAt("Josh_OhhMan", player.transform.position, 100);
                                break;
                            case 5:
                                soundMan.PlaySfxAt("Josh_ThankYou", player.transform.position, 100);
                                break;
                            case 6:
                                soundMan.PlaySfxAt("Josh_YouAreALovelyPerson", player.transform.position, 100);
                                break;
                            case 7:
                                soundMan.PlaySfxAt("Josh_YouShouldFeelGoodAboutYourself", player.transform.position, 100);
                                break;
                            default:
                                soundMan.PlaySfxAt("Josh_HaveYouEverWonderedWhatSlothsLookLikeWhenTheyreDrunk", player.transform.position, 100);
                                break;
                        }
                    }
                    else if (currentPatron.name.Contains("John"))
                    {
                        soundIndex = Random.Range(0, 7); //7 positive John sounds

                        switch (soundIndex)
                        {
                            case 0:
                                soundMan.PlaySfxAt("John_CheersMate0", player.transform.position, 100);
                                break;
                            case 1:
                                soundMan.PlaySfxAt("John_CheersMate1", player.transform.position, 100);
                                break;
                            case 2:
                                soundMan.PlaySfxAt("John_OhRightOnTime", player.transform.position, 100);
                                break;
                            case 3:
                                soundMan.PlaySfxAt("John_OiGiveMeOneOfThose", player.transform.position, 100);
                                break;
                            case 4:
                                soundMan.PlaySfxAt("John_OyThanks", player.transform.position, 100);
                                break;
                            case 5:
                                soundMan.PlaySfxAt("John_OyThanksMateYouSaint", player.transform.position, 100);
                                break;
                            case 6:
                                soundMan.PlaySfxAt("John_YeahIllHaveAnotherOneOfThose", player.transform.position, 100);
                                break;
                            default:
                                soundMan.PlaySfxAt("John_OyThanksMateYouSaint", player.transform.position, 100);
                                break;
                        }
                    }
                    else if (currentPatron.name.Contains("Joel"))
                    {
                        soundIndex = Random.Range(0, 4); //4 positive Joel sounds

                        switch (soundIndex)
                        {
                            case 0:
                                soundMan.PlaySfxAt("Joel_MouthTick_Delicious", player.transform.position, 100);
                                break;
                            case 1:
                                soundMan.PlaySfxAt("Joel_Another", player.transform.position, 100);
                                break;
                            case 2:
                                soundMan.PlaySfxAt("Joel_Hallo", player.transform.position, 100);
                                break;
                            case 3:
                                soundMan.PlaySfxAt("Joel_OhThankYou", player.transform.position, 100);
                                break;
                            default:
                                soundMan.PlaySfxAt("Joel_MouthTick_Delicious", player.transform.position, 100);
                                break;
                        }
                    }
                }
                #endregion

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
