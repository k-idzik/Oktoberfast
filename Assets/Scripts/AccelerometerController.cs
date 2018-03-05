using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Utilize the mobile accelerometer as a controller
public class AccelerometerController : MonoBehaviour
{
    //DEBUG
    private UnityEngine.UI.Text[] tiltAmounts = new UnityEngine.UI.Text[2]; //UI text

    // lists for holding beer and steins
    private List<UnityEngine.UI.Image> steins;
    private List<UnityEngine.UI.Image> beers;

    // rate at which stein turns
    public float steinRotateSpeed = 1f;

    // rate at which beer spills
    public float beerSpilledRate = 0.5f;

    // values to hold upper corners of the steins and beers
    private Vector3[] steinCorners;
    private Vector3[] beerCorners;

    float minRotation = -90;
    float maxRotation = 90;

    //Player Attributes
	private float maxSpeed; //This is the original speed player moves at that we use to speed character up till after braking
    [SerializeField] private float speed = 5.0f; //How fast player is currently moving
	[SerializeField] private float brakeForce = 0.5f;	//Amount to slow player down by while screen is pressed
	[SerializeField] private float minSpeed = 1.0f; 	//Minimum amount of speed character can move at
	[SerializeField] private float turnSpeed = 1.0f;    //How fast player turns

    //Screen/Device Variables
    private bool screenTouched;
    Vector3 accelerometer; //holds acceleration for device
	//private Vector3 calibrationVec; //used for left right calibration

    //Use this for initialization
    void Start()
    {
        //DEBUG
        tiltAmounts = GetComponentsInChildren<UnityEngine.UI.Text>(); //Pull in the UI text

        // populate steins array and beers array
        steins = new List<UnityEngine.UI.Image>();
        beers = new List<UnityEngine.UI.Image>();
        foreach (UnityEngine.UI.Image i in GetComponentsInChildren<UnityEngine.UI.Image>())
        {
            if (i.tag == "Stein")
                steins.Add(i);

            else if (i.tag == "Beer")
                beers.Add(i);
        }

        // initialize corners arrays
        steinCorners = new Vector3[4];
        beerCorners = new Vector3[4];

        //Keep the screen on
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        screenTouched = false; //Indicates whether screen is touched

		//initialize Player movement
		maxSpeed = speed;
		

       // calibrationVec = Input.acceleration;
    }

    //Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR //Debug controls
        transform.Translate(Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0, 0); //Move the object that this script is attached to

        steins[0].transform.Rotate(Vector3.forward, steinRotateSpeed * -Input.GetAxis("Horizontal"));
        beers[0].transform.Rotate(Vector3.forward, steinRotateSpeed * Input.GetAxis("Horizontal"));
#endif
        //Get accelerometer vector
        accelerometer = Input.acceleration;

        //Ignore really small movements
        if (Mathf.Abs(accelerometer.x) < .015f)
            accelerometer.x = 0;
		
		//Move Player
		Move();

        steins[0].transform.Rotate(Vector3.forward, 12 * -accelerometer.x * Time.deltaTime);
        beers[0].transform.Rotate(Vector3.forward, 12 * accelerometer.x * Time.deltaTime);

        // after stein rotates perform check to see if any beer has spilt
        // first, retrieve upper right and upper left corners of stein and beer
        // 1 = top left
        // 2 = top right
        steins[0].rectTransform.GetWorldCorners(steinCorners);
        beers[0].rectTransform.GetWorldCorners(beerCorners);

        // if either corner of the stein is greater than the corner of the beer the beer should spill
        if (steinCorners[2].y < beerCorners[2].y)
        {
            beers[0].transform.Translate(new Vector3(0, -beerSpilledRate * (beerCorners[2].y - steinCorners[2].y), 0));
        }
        else if (steinCorners[1].y < beerCorners[1].y)
        {
            beers[0].transform.Translate(new Vector3(0, -beerSpilledRate * (beerCorners[1].y - steinCorners[1].y), 0));
        }

        //DEBUG
        tiltAmounts[0].text = "Touches: " + Input.touchCount;
        tiltAmounts[1].text = "Speed: " + speed;

    }

    public void Move()
    {
        if (Input.touchCount > 0)
        {
            //Get Touch Id
            Touch screenTouch = Input.GetTouch(0); //gets the first touch

            switch (screenTouch.phase)
            {
                case TouchPhase.Began:
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    screenTouched = true; //Screen has been touched;
                    speed -= brakeForce;

                    if (speed < minSpeed)
                        speed = minSpeed;

                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    screenTouched = false;
                    break;
            }
        }
        else
        {
            speed += brakeForce;

            if (speed > maxSpeed)
                speed = maxSpeed;
        }
        transform.Translate(accelerometer.x * turnSpeed * Time.deltaTime, 0, speed * Time.deltaTime);

		
    }

}