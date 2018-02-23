using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Utilize the mobile accelerometer as a controller
public class AccelerometerController : MonoBehaviour
{
    //DEBUG
    private UnityEngine.UI.Text[] tiltAmounts = new UnityEngine.UI.Text[2]; //UI text

    //Use this for initialization
    void Start()
    {
        //DEBUG
        tiltAmounts = GetComponentsInChildren<UnityEngine.UI.Text>(); //Pull in the UI text
    }
	
	//Update is called once per frame
	void Update()
    {
        Vector3 accelerometer = Input.acceleration * .1f; //Get the acceleration, dull it down a bit

        //Ignore really small movements
        if (Mathf.Abs(accelerometer.x) < .015f)
            accelerometer.x = 0;
        if (Mathf.Abs(accelerometer.z) < .015f)
            accelerometer.z = 0;

        transform.Translate(accelerometer.x, 0, -accelerometer.z); //Move the object that this script is attached to

        //DEBUG
        tiltAmounts[0].text = "X: " + accelerometer.x;
        tiltAmounts[1].text = "Z: " + accelerometer.z;
    }
}