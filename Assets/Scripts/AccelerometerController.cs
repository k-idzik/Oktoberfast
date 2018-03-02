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
    }
	
	//Update is called once per frame
	void Update()
    {
#if UNITY_EDITOR //Debug controls
        transform.Translate(Input.GetAxis("Horizontal") * .1f, 0, Input.GetAxis("Vertical") * .1f); //Move the object that this script is attached to
        steins[0].transform.Rotate(Vector3.forward, steinRotateSpeed * -Input.GetAxis("Horizontal"));
        beers[0].transform.Rotate(Vector3.forward, steinRotateSpeed * Input.GetAxis("Horizontal"));
#else
        Vector3 accelerometer = Input.acceleration * .1f; //Get the acceleration, dull it down a bit

        //Ignore really small movements
        if (Mathf.Abs(accelerometer.x) < .015f)
            accelerometer.x = 0;
        if (Mathf.Abs(accelerometer.z) < .015f)
            accelerometer.z = 0;

        transform.Translate(accelerometer.x, 0, -accelerometer.z); //Move the object that this script is attached to

        if (Mathf.Abs(accelerometer.x) < .03f)
            accelerometer.x = 0;

        steins[0].transform.Rotate(Vector3.forward, steinRotateSpeed * -accelerometer.x);
        beers[0].transform.Rotate(Vector3.forward, steinRotateSpeed * accelerometer.x);

        //DEBUG
        tiltAmounts[0].text = "X: " + accelerometer.x;
        tiltAmounts[1].text = "Z: " + accelerometer.z;
#endif
    }
}