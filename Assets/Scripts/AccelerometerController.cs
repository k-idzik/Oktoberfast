using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Utilize the mobile accelerometer as a controller
public class AccelerometerController : MonoBehaviour
{
    //DEBUG
    private UnityEngine.UI.Text[] tiltAmounts; //UI text

    // lists for holding beer and steins
    private List<UnityEngine.UI.Image> steins;
    private List<UnityEngine.UI.Image> beers;

    // the initial point to rotate the stein and beer back to
    private Quaternion initSteinRotation;
    private Quaternion initBeerRotation;
    private Vector3 initBeerPos;

    // rate at which stein turns
    public float steinRotateSpeed = 1f;

    // rate at which beer spills
    public float beerSpilledRate = 0.5f;

    // rate at which stein and beer rotate to normal
    public float returnRotationRate = 8f;

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
	[SerializeField] private float turnSpeed = 2.0f;    //How fast player turns

    //Screen/Device Variables
    private bool screenTouched;
    float accelerometer; //holds acceleration for device

    //Use this for initialization
    void Start()
    {
        //DEBUG
        tiltAmounts = GameObject.Find("DebugGroup").GetComponentsInChildren<UnityEngine.UI.Text>(); //Pull in the UI text

        // populate steins array and beers array
        steins = new List<UnityEngine.UI.Image>();
        beers = new List<UnityEngine.UI.Image>();
        foreach (UnityEngine.UI.Image i in GameObject.Find("GameUI").GetComponentsInChildren<UnityEngine.UI.Image>())
        {
            if (i.tag == "Stein")
                steins.Add(i);

            else if (i.tag == "Beer")
                beers.Add(i);
        }

        // initialize stein and beer rotation
        initSteinRotation = steins[0].transform.rotation;
        initBeerRotation = beers[0].transform.rotation;
        initBeerPos = beers[0].transform.position;

        // initialize corners arrays
        steinCorners = new Vector3[4];
        beerCorners = new Vector3[4];

        //Keep the screen on
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        screenTouched = false; //Indicates whether screen is touched

		//initialize Player movement
		maxSpeed = speed;
    }

    //Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR //Debug controls
        MovePC();

        // simple way to prevent beer from roatating
        beers[0].transform.rotation = initBeerRotation;

        //Let steins rotate
        steins[0].transform.Rotate(Vector3.forward, steinRotateSpeed * -Input.GetAxis("Horizontal") * Time.deltaTime);

//DON'T REMOVE THIS
//OR YE SHALL FACE THE WRATH OF BRÜCE ON JUICE
#else
        // simple way to prevent beer from roatating
        beers[0].transform.rotation = initBeerRotation;
        //beers[0].transform.position.Set(0, beers[0].transform.position.y, beers[0].transform.position.z);

        //Get accelerometer vector
        accelerometer = Input.acceleration.x;

        //Ignore really small movements
        if (Mathf.Abs(accelerometer) < .015f)
            accelerometer = 0;
		
		//Move Player
		Move();

        //Let steins rotate
        steins[0].transform.Rotate(Vector3.forward, 12 * -accelerometer * Time.deltaTime);
#endif
        // after stein rotates perform check to see if any beer has spilt
        // first, retrieve upper right and upper left corners of stein and beer
        // 1 = top left
        // 2 = top right
        steins[0].rectTransform.GetWorldCorners(steinCorners);
        beers[0].rectTransform.GetWorldCorners(beerCorners);

        // if either corner of the stein is greater than the corner of the beer the beer should spill
        if (steinCorners[2].y < beerCorners[2].y)
        {
            beers[0].rectTransform.Translate(new Vector3(0, -1, 0) * beerSpilledRate * (beerCorners[2].y - steinCorners[2].y) * Time.deltaTime, Space.Self);
            beers[0].rectTransform.anchoredPosition = (new Vector3(0, beers[0].rectTransform.localPosition.y, 0));
        }
        else if (steinCorners[1].y < beerCorners[1].y)
        {
            beers[0].rectTransform.Translate(new Vector3(0, -1, 0) * beerSpilledRate * (beerCorners[1].y - steinCorners[1].y) * Time.deltaTime, Space.Self);
            beers[0].rectTransform.anchoredPosition = (new Vector3(0, beers[0].rectTransform.localPosition.y, 0));
        }

        //Display framerate for debug
        tiltAmounts[0].text = "FPS: " + (1 / Time.deltaTime).ToString();
    }

    //Movement on mobile
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

        // stein should rotate back to center
        if (accelerometer == 0f)
        {
            steins[0].transform.rotation = Quaternion.RotateTowards(steins[0].transform.rotation, initSteinRotation, returnRotationRate * Time.deltaTime);
            beers[0].transform.rotation = Quaternion.RotateTowards(beers[0].transform.rotation, initBeerRotation, returnRotationRate * Time.deltaTime);
        }

        //Move forward
        transform.Translate(accelerometer * turnSpeed * Time.deltaTime, 0, speed * Time.deltaTime);
    }

    //Movement on PC
    public void MovePC()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            speed -= brakeForce;

            if (speed < minSpeed)
                speed = minSpeed;
        }
        else
        {
            speed += brakeForce;

            if (speed > maxSpeed)
                speed = maxSpeed;
        }

        // stein should rotate back to center
        if (Input.GetAxis("Horizontal") == 0f)
            steins[0].transform.rotation = Quaternion.RotateTowards(steins[0].transform.rotation, initSteinRotation, returnRotationRate * Time.deltaTime);

        //Move forward
        transform.Translate(Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0, speed * Time.deltaTime);
    }

    //Vibrate when the player is in range of the patron
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Patron")
        {
            Handheld.Vibrate();
        }
    }

    //When the player taps, serve the patron
    private void OnTriggerStay(Collider coll)
    {
#if UNITY_EDITOR //Debug controls
        if (Input.GetKey(KeyCode.B))
        {
            Debug.Log("Beer delivered");
        }
#else
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {

        }
#endif
    }
}