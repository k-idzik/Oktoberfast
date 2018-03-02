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
    bool forward = false;
    bool backward = false;

    Vector3 calibrationVec;
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

        calibrationVec = Input.acceleration;
    }

    //Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR //Debug controls
        transform.Translate(Input.GetAxis("Horizontal") * .1f, 0, Input.GetAxis("Vertical") * .1f); //Move the object that this script is attached to

        steins[0].transform.Rotate(Vector3.forward, steinRotateSpeed * -Input.GetAxis("Horizontal"));
        beers[0].transform.Rotate(Vector3.forward, steinRotateSpeed * Input.GetAxis("Horizontal"));
#endif
        MoveForward();
        Vector3 accelerometer = (Input.acceleration - calibrationVec)* .1f; //Get the acceleration, dull it down a bit
        
        //Ignore really small movements
        if (Mathf.Abs(accelerometer.x) < .015f)
            accelerometer.x = 0;
        if (Mathf.Abs(accelerometer.z) < .015f)
            accelerometer.z = 0;

        //if(Input.acceleration.x < calibrationVec.x)
        transform.Translate(accelerometer.x, 0, -accelerometer.z); //Move the object that this script is attached to

        if (Mathf.Abs(accelerometer.x) < .04f)
            accelerometer.x = 0;

        steins[0].transform.Rotate(Vector3.forward, 12 * -accelerometer.x);
        beers[0].transform.Rotate(Vector3.forward, 12 * accelerometer.x);

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
        tiltAmounts[0].text = "X: " + accelerometer.x;
        tiltAmounts[1].text = "Z: " + accelerometer.z;
    }

    public void MoveForward()
    {
        if(forward)
            transform.Translate(0, 0, 5 * Time.deltaTime);
        if(backward)
            transform.Translate(0, 0, -5 * Time.deltaTime);
    }

    public void SetForward(bool status)
    {
        forward = status;
    }

    public void setBackward(bool status) { backward = status; }
}