using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Utilize the mobile accelerometer as a controller
public class Player : MonoBehaviour
{
    //DEBUG
    //private UnityEngine.UI.Text[] tiltAmounts; //UI text

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

    //float minRotation = -90;
    //float maxRotation = 90;

    //Player Attributes
	private float maxSpeed; //This is the original speed player moves at that we use to speed character up till after braking
    private float speed = 5.0f; //How fast player is currently moving
	private float brakeForce = 0.5f;	//Amount to slow player down by while screen is pressed
	private float minSpeed = 1.0f; 	//Minimum amount of speed character can move at
	private float turnSpeed = 50.0f;    //How fast player turns

    //Screen/Device Variables
    private bool screenTouched;
    private float accelerometer; //holds acceleration for device
    private float beerSpillAccelerometer; //Separate beer spill acceleration
    private float accelerometerLimit = .15f; //Minimun acceleration needed for turn on mobile
    //private float beerSpillAccelerometerLimit = .45f; //Separate max beer spill acceleration

    //Beer delivery
    private UnityEngine.UI.Text[] deliverBeerText;
    private UnityEngine.UI.Text patronsServedUI;
    private float shakeAmount = 3; //UI effect
    private int beersServed = 0; //Number of patrons served
    private bool isBeerServed = false; //If the current beer has been served
    private double beerTipAmount = 0; //Total tip amount
    private double maxBeerTilt = 180; //Max beer spilled, factors into tipping
    private Vector3 initialBeerPosition; //The initial position of the beer, used in beer refilling

    public bool obstacleCollision = false; //If the player is colliding with a table
    public bool canCollide = true; //If the player can collide with a table (prevents multiple collisions/spills on the same object)

    //Properties
    public int BeersServed
    {
        get
        {
            return beersServed;
        }
    }
    public double BeerTipAmount
    {
        get
        {
            //Round that before you give it back
            //Ain't nobody got 1/5000th of a cent
            return System.Math.Round(beerTipAmount, 2);
        }
        set
        {
            beerTipAmount = value;
        }
    }

    //Use this for initialization
    void Start()
    {
        //DEBUG
        //tiltAmounts = GameObject.Find("DebugGroup").GetComponentsInChildren<UnityEngine.UI.Text>(); //Pull in the UI text

        deliverBeerText = GameObject.Find("FlyBys").GetComponentsInChildren<UnityEngine.UI.Text>(); //Pull in the UI text for beer delivery

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

        initialBeerPosition = beers[0].transform.position; //Save the initial beer position

        //Get the patron UI text
        patronsServedUI = GameObject.FindGameObjectWithTag("NumServed").GetComponent<UnityEngine.UI.Text>();
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
        beers[0].transform.position.Set(0, beers[0].transform.position.y, beers[0].transform.position.z);

        //Get accelerometer vector
        accelerometer = Input.acceleration.x;
        beerSpillAccelerometer = accelerometer;

        //Ignore really small movements
        if (Mathf.Abs(accelerometer) < accelerometerLimit)
            accelerometer = 0;

        //Separate for beer, more tolerance
        //if (Mathf.Abs(beerSpillAccelerometer) < beerSpillAccelerometerLimit)
        //    beerSpillAccelerometer = 0;

        //Move Player
        Move();

        //Let steins rotate
        steins[0].transform.Rotate(Vector3.forward, steinRotateSpeed * -beerSpillAccelerometer * Time.deltaTime);
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

        CalculateTiltFactor(); //Calculate the tilt factor

        //Display framerate for debug
        //tiltAmounts[0].text = "FPS: " + (1 / Time.deltaTime).ToString();
        //tiltAmounts[1].text = Input.acceleration.x.ToString();
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

        //// stein should rotate back to center
        if (accelerometer == 0f)
        {
            steins[0].transform.rotation = Quaternion.RotateTowards(steins[0].transform.rotation, initSteinRotation, returnRotationRate * Time.deltaTime);
            beers[0].transform.rotation = Quaternion.RotateTowards(beers[0].transform.rotation, initBeerRotation, returnRotationRate * Time.deltaTime);
        }

        //Move forward
        transform.Translate(0, 0, speed * Time.deltaTime);

        //Rotate
        transform.Rotate(Vector3.up, accelerometer * turnSpeed * Time.deltaTime);
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

        //// stein should rotate back to center
        if (Input.GetAxis("Horizontal") == 0f)
            steins[0].transform.rotation = Quaternion.RotateTowards(steins[0].transform.rotation, initSteinRotation, returnRotationRate * Time.deltaTime);

        //Move forward
        transform.Translate(0, 0, speed * Time.deltaTime);

        //Rotate
        transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime);
    }
    
    //Checks beer tilt for tip calculation
    private void CalculateTiltFactor()
    {
        //Precalculate tilt factor
        double tiltFactor = Mathf.Abs(steins[0].transform.rotation.eulerAngles.z - 180);

        //Keep a tally of the amount of beer spilled
        if (tiltFactor < maxBeerTilt)
            maxBeerTilt = tiltFactor;

        Debug.Log(maxBeerTilt);
    }

    //Refill beer after it has been served
    private void RefillBeer()
    {
        //Reset beer serving
        isBeerServed = false;
        beers[0].transform.rotation = Quaternion.Euler(0, 0, 0);
        steins[0].transform.rotation = Quaternion.Euler(0, 0, 0);
        beers[0].transform.position = initialBeerPosition; //Can't be magic numbers, depends on screen resolution
        maxBeerTilt = 180;
    }

    //Entering trigger with patron
    private void OnTriggerEnter(Collider coll)
    {
        //Make sure this is the patron
        if (coll.tag == "Patron")
        {
            //Enable the beer text
            deliverBeerText[0].enabled = true;
            deliverBeerText[1].enabled = true;
        }

        if (coll.gameObject.tag == "Obstacle" && canCollide)
        {
            Handheld.Vibrate(); //Vibrate to indicate collision with table

            //Stop movement
            //obstacleCollision = true;
            canCollide = false;
            StartCoroutine(WaitForNextTableCollision()); //Make sure no immediate collisions occur again
            speed = 0;

            //Spill beer
            beers[0].rectTransform.Translate(new Vector3(0, -25, 0), Space.Self);
            beers[0].rectTransform.anchoredPosition = (new Vector3(0, beers[0].rectTransform.localPosition.y, 0));

            maxBeerTilt -= 25; //Automatically lose points when you hit a table (should be 50 cents)
        }
    }

    private void OnTriggerStay(Collider coll)
    {
        //When the player taps, serve the patron
        if (coll.tag == "Patron")
        {
            Patron patronServed = coll.GetComponent<Patron>();

            if (deliverBeerText[0].fontSize < 300)
            {
                deliverBeerText[0].fontSize += 30;
                deliverBeerText[1].fontSize += 30;
            }

            //Precalculate shake value
            float shakeTextZ = Mathf.Sin(Time.time) * shakeAmount;

            //Shake beer delivery text to make it stand out
            deliverBeerText[0].transform.Rotate(0, 0, shakeTextZ);
            deliverBeerText[1].transform.Rotate(0, 0, -shakeTextZ);

            shakeAmount *= -1; //Flip to make the beer shake the other way the next frame

#if UNITY_EDITOR //Debug controls
            if (Input.GetKey(KeyCode.B) && !isBeerServed && patronServed.PatronNum == BeersServed + 1)
#else
            if (Input.GetTouch(0).phase == TouchPhase.Began && !isBeerServed  && patronServed.PatronNum == BeersServed + 1)
#endif
            {
                //Deliver beer
                beersServed++;
                patronsServedUI.text = "Patrons Served: " + beersServed;
                isBeerServed = true;

                //Don't tip bad service
                if (maxBeerTilt < 100)
                    maxBeerTilt = 0;

                beerTipAmount += (maxBeerTilt * 2) / 100; //Calculate the tip

                //Disable the beer text
                deliverBeerText[0].enabled = false;
                deliverBeerText[1].enabled = false;

                //Set Next Patron to Serve 
                MenuManager.Instance.SwitchPatronTarget(patronServed.NextPatron);
            }
        }
    }

    //Exit trigger with patron
    private void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "Patron")
        {
            //Disable the beer text
            deliverBeerText[0].enabled = false;
            deliverBeerText[1].enabled = false;

            RefillBeer(); //Refill the stein
        }

        //if (coll.gameObject.tag == "Obstacle")
        //{
        //    obstacleCollision = false; //Resume movement

        //    StartCoroutine(WaitForNextTableCollision()); //Make sure no immediate collisions occur again
        //}
    }

    //Coroutine, wait until you can collide again
    private IEnumerator WaitForNextTableCollision()
    {
        yield return new WaitForSeconds(1.50f);

        canCollide = true;
    }
}