using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SoundManager : Singleton<SoundManager>
{
    //FIELDS
	private AudioSource sfxSource; //what'll play effects
	private Dictionary<string, AudioClip> sfxLibrary = new Dictionary<string, AudioClip>(); //sound effect library

    private AudioSource musicSource; //source for looping background music
    private Dictionary<string, AudioClip> musicLibrary = new Dictionary<string, AudioClip>(); //music library

    protected SoundManager() { }

	void Awake()
	{
        //dont touch me when we load a new scene
		DontDestroyOnLoad(this);
	}

	void Start()
	{
		//add audio sources to sound manager obj
		sfxSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();

        //add sounds to the lib
       musicLibrary.Add("background", Resources.Load("sfx/Background") as AudioClip);
       musicLibrary.Add("background_reverse", Resources.Load("sfx/Background_ReverseReverb") as AudioClip);
        musicLibrary.Add("Josh&Joel_BoothShenanigans", Resources.Load("sfx/Josh&Joel_BoothShenanigans") as AudioClip);

        sfxLibrary.Add("Charles_AhDasIsGutBeer", Resources.Load("sfx/Charles_AhDasIsGutBeer") as AudioClip);
        sfxLibrary.Add("Charles_BringMeOneOfThose", Resources.Load("sfx/Charles_BringMeOneOfThose") as AudioClip);
        sfxLibrary.Add("Charles_CanIHaveABeer", Resources.Load("sfx/Charles_CanIHaveABeer") as AudioClip);
        sfxLibrary.Add("Charles_Danke", Resources.Load("sfx/Charles_Danke") as AudioClip);
        sfxLibrary.Add("Charles_DankeMeinFreund", Resources.Load("sfx/Charles_DankeMainFreund") as AudioClip);
        sfxLibrary.Add("Charles_DasIstNichtGut", Resources.Load("sfx/Charles_DasIstNichtGut") as AudioClip);
        sfxLibrary.Add("Charles_HalloIchMochteEinDunkleLager", Resources.Load("sfx/Charles_hallowIchMochteEinDunkleLager") as AudioClip);
        sfxLibrary.Add("Charles_IHopeYoureNotExpectingAGoodTip", Resources.Load("sfx/Charles_IHopeYoureNotExpectingAGoodTip") as AudioClip);
        sfxLibrary.Add("Charles_StopSpillingMyBeer", Resources.Load("sfx/Charles_StopSpillingMyBeer") as AudioClip);
        sfxLibrary.Add("Charles_ThankYouForThat", Resources.Load("sfx/Charles_ThankYouForThat") as AudioClip);
        sfxLibrary.Add("Charles_ThankYouMyFriend", Resources.Load("sfx/Charles_ThankYouMyFriend") as AudioClip);
        sfxLibrary.Add("Charles_YouWereLate", Resources.Load("sfx/Charles_YouWereLate") as AudioClip);
        sfxLibrary.Add("Charles_ZeDanke", Resources.Load("sfx/Charles_ZeDanke") as AudioClip);
        sfxLibrary.Add("Joel_MouthTick_Delicious", Resources.Load("sfx/Joel_MouthTick_Delicious") as AudioClip);
        sfxLibrary.Add("Joel_Alright0", Resources.Load("sfx/Joel_Alright0") as AudioClip);
        sfxLibrary.Add("Joel_Alright1", Resources.Load("sfx/Joel_Alright1") as AudioClip);
        sfxLibrary.Add("Joel_Another", Resources.Load("sfx/Joel_Another") as AudioClip);
        sfxLibrary.Add("Joel_Hallo", Resources.Load("sfx/Joel_Hallo") as AudioClip);
        sfxLibrary.Add("Joel_OhThankYou", Resources.Load("sfx/Joel_OhThankYou") as AudioClip);
        sfxLibrary.Add("Joel_SoUhhYouDontWantATipThen", Resources.Load("sfx/Joel_SoUhhYouDontWantATipThen") as AudioClip);
        sfxLibrary.Add("Joel_ThatsSadThatsActuallyReallyDisappointing", Resources.Load("sfx/Joel_ThatsSadThatsActuallyReallyDisappointing") as AudioClip);
        sfxLibrary.Add("Joel_ThatWasHorrible", Resources.Load("sfx/Joel_ThatWasHorrible") as AudioClip);
        sfxLibrary.Add("Joel_ThisIsLikeNoBeerLeft", Resources.Load("sfx/Joel_ThisIsLikeNoBeerLeft") as AudioClip);
        sfxLibrary.Add("Joel_ThisIsntWhatIOrdered0", Resources.Load("sfx/Joel_ThisIsntWhatIOrdered0") as AudioClip);
        sfxLibrary.Add("Joel_ThisIsntWhatIOrdered1", Resources.Load("sfx/Joel_ThisIsntWhatIOrdered1") as AudioClip);
        sfxLibrary.Add("Joel_Uhh", Resources.Load("sfx/Joel_Uhh") as AudioClip);
        sfxLibrary.Add("Joel_YoureKindOftakingALongTime", Resources.Load("sfx/Joel_YoureKindOfTakingALongTime") as AudioClip);
        sfxLibrary.Add("Joel_YouSpilliedAllMyBeer", Resources.Load("sfx/Joel_YouSpilledAllMyBeer") as AudioClip);
        sfxLibrary.Add("John_CheersMate0", Resources.Load("sfx/John_CheersMate0") as AudioClip);
        sfxLibrary.Add("John_CheersMate1", Resources.Load("sfx/John_CheersMate1") as AudioClip);
        sfxLibrary.Add("John_HeyWatchWhereYoureSpillingThat", Resources.Load("sfx/John_HeyWatchWhereYoureSpillingThat") as AudioClip);
        sfxLibrary.Add("John_ILikeToActuallyDrinkWhatIOrder", Resources.Load("sfx/John_ILikeToActuallyDrinkWhatIOrder") as AudioClip);
        sfxLibrary.Add("John_OhRightOnTime", Resources.Load("sfx/John_OhRightOnTime") as AudioClip);
        sfxLibrary.Add("John_OhSorryBoutThatDozedOffWithHowLingItTook", Resources.Load("sfx/John_OhSorryBoutThatDozedOffWithHowLongItTook") as AudioClip);
        sfxLibrary.Add("John_OiGiveMeOneOfThose", Resources.Load("sfx/John_OiGiveMeOneOfThose") as AudioClip);
        sfxLibrary.Add("John_OiYoureTakingAWhile", Resources.Load("sfx/John_OiYoureTakingAWhile") as AudioClip);
        sfxLibrary.Add("John_OyThanks", Resources.Load("sfx/John_OyThanks") as AudioClip);
        sfxLibrary.Add("John_OyThanksMateYouSaint", Resources.Load("sfx/John_OyThanksMateYouSaint") as AudioClip);
        sfxLibrary.Add("John_Snore", Resources.Load("sfx/John_Snore") as AudioClip);
        sfxLibrary.Add("John_WhyIEvenComeHere", Resources.Load("sfx/John_WhyIEvenComeHere") as AudioClip);
        sfxLibrary.Add("John_SnoreOhSorryBoutThatDozedOffWithHowLingItTook", Resources.Load("sfx/John_SnoreOhSorryBoutThatDozedOffWithHowLongItTook") as AudioClip);
        sfxLibrary.Add("John_YeahIdLikeMyBeerFullThanks", Resources.Load("sfx/John_YeahIdLikeMyBeerFullThanks") as AudioClip);
        sfxLibrary.Add("John_YeahIllHaveAnotherOneOfThose", Resources.Load("sfx/John_YeahIllHaveAnotherOneOfThose") as AudioClip);
        sfxLibrary.Add("Josh_BringMeABeer0", Resources.Load("sfx/Josh_BringMeABeer0") as AudioClip);
        sfxLibrary.Add("Josh_BringMeABeer1", Resources.Load("sfx/Josh_BringMeABeer1") as AudioClip);
        sfxLibrary.Add("Josh_BringMeMyBeer0", Resources.Load("sfx/Josh_BringMeMyBeer0") as AudioClip);
        sfxLibrary.Add("Josh_BringMeMyBeer1", Resources.Load("sfx/Josh_BringMeMyBeer1") as AudioClip);
        sfxLibrary.Add("Josh_BringMeMyBeerWench", Resources.Load("sfx/Josh_BringMeMyBeerWench") as AudioClip);
        sfxLibrary.Add("Josh_CanYouNot", Resources.Load("sfx/Josh_CanYouNot") as AudioClip);
        sfxLibrary.Add("Josh_GetBetter", Resources.Load("sfx/Josh_GetBetter") as AudioClip);
        sfxLibrary.Add("Josh_GetMeTwoBeersPlease", Resources.Load("sfx/Josh_GetMeTwoBeersPlease") as AudioClip);
        sfxLibrary.Add("Josh_HaveYouEverWonderedWhatSlothsLookLikeWhenTheyreDrunk", Resources.Load("sfx/Josh_HaveYouEverWonderedWhatSlothsLookLikeWhenTheyreDrunk") as AudioClip);
        sfxLibrary.Add("Josh_HeyYouBroughtMeMyBeerGoodJob", Resources.Load("sfx/Josh_HeyYouBroughtMeMyBeerGoodJob") as AudioClip);
        sfxLibrary.Add("Josh_Mmm", Resources.Load("sfx/Josh_Mmm") as AudioClip);
        sfxLibrary.Add("Josh_OhhMan", Resources.Load("sfx/Josh_OhhMan") as AudioClip);
        sfxLibrary.Add("Josh_OhNoMyBeersOnTheGroundThatsOkay", Resources.Load("sfx/Josh_OhNoMyBeersOnTheGroundThatsOkay") as AudioClip);
        sfxLibrary.Add("Josh_StopThisIDontLikeThis", Resources.Load("sfx/Josh_StopThisIDontLikeThis") as AudioClip);
        sfxLibrary.Add("Josh_ThankYou", Resources.Load("sfx/Josh_ThankYou") as AudioClip);
        sfxLibrary.Add("Josh_WhyMustYouSpill", Resources.Load("sfx/Josh_WhyMustYouSpill") as AudioClip);
        sfxLibrary.Add("Josh_YouAreALovelyPerson", Resources.Load("sfx/Josh_YouAreALovelyPerson") as AudioClip);
        sfxLibrary.Add("Josh_YouShouldFeelGoodAboutYourself", Resources.Load("sfx/Josh_YouShouldFeelGoodAboutYourself") as AudioClip);

        //loop background music
        musicSource.loop = true; //turn on looping
       musicSource.clip = musicLibrary["background"]; //set default song
        ChangeMusicVolume(0);
        musicSource.Play(); //play music
	}

    /// <summary>
    /// Plays sfx once so it doesnt overlap itself
    /// </summary>
    /// <param name="name">Name of sound in dictionary</param>
    /// <param name="volume">Volume of sound</param>
    public void PlaySfxOnce(string name, int volume)
    {
        //PlaySfxAt(name, this.gameObject.transform.position, volume);
        //check if its currently playing
        if (!GameObject.Find("One shot audio"))
        {
            //play sound again
            PlaySfxAt(name, this.gameObject.transform.position, volume);
        }
    }

    /// <summary>
    /// chnage the background music
    /// </summary>
    /// <param name="name">key of the song to play</param>
    public void ChangeSong(string name)
    {
        musicSource.Stop(); //stop current song

        musicSource.clip = musicLibrary[name]; //set song
        musicSource.Play(); //play song
    }

    /// <summary>
    /// Plays sound at volume
    /// </summary>
    /// <param name="name">key for sound to be played</param>
    /// /// <param name="volume">volume of this sound</param> 
    public void PlaySfx(string name, int volume)
    { 
        //convert volume into percentage
        float vol = (float)(volume / 100.00);

        //scale volume by sourceVolume
        vol *= sfxSource.volume;

        //play sound from lib once
		sfxSource.PlayOneShot(sfxLibrary[name], vol);
	}

    /// <summary>
    /// Plays sound from a specific position in world space by creating its own source
    /// </summary>
    /// <param name="name">key for sound to be played</param>
    /// <param name="position">world cordinates to be played from</param>
    /// <param name="volume">volume of this sound</param> 
    public void PlaySfxAt(string name, Vector3 position, int volume)
    {
        //convert volume into percentage
        float vol = (float)(volume / 100.00);

        //scale volume by sourceVolume
        vol *= sfxSource.volume;

        //doesnt use source bc it creates its own for this
        AudioSource.PlayClipAtPoint(sfxLibrary[name], position, vol);
    }

    /// <summary>
    /// changes sfx volume
    /// </summary>
    /// <param name="volume">number 0-100 of volume</param>
    public void ChangeSfxVolume(int volume)
    {
        //chnage volume into a float for the actual volume- and set to sfxVolume
        sfxSource.volume = (float)(volume / 100.00);
    }

    /// <summary>
    /// changes music volume
    /// </summary>
    /// <param name="volume">number 0-100 of volume</param>
    public void ChangeMusicVolume(int volume)
    {
        //chnage volume into a float for the actual volume- and set to sfxVolume
        musicSource.volume = (float)(volume / 100.00);
    }
}
