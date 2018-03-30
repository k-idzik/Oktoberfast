using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScreenAnimation : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image[] tiltPhone = new UnityEngine.UI.Image[3]; //Mobile tilt images
    [SerializeField] private UnityEngine.UI.Image[] tapPhone = new UnityEngine.UI.Image[2]; //Mobile tap images
    [SerializeField] private UnityEngine.UI.Text[] deliverBeerText = new UnityEngine.UI.Text[2]; //Beer delivery text
    private int shakeAmount = 3;
    private int timer = 0;

	//Use this for initialization
	void Start()
    {
		
	}
	
	//Fixed updates
	void FixedUpdate()
    {
        //Change which image is active
        if (timer < 30 || (timer >= 60 && timer < 90))
        {
            tiltPhone[0].enabled = true;
            tiltPhone[1].enabled = false;
            tiltPhone[2].enabled = false;

            tapPhone[0].enabled = true;
            tapPhone[1].enabled = false;
        }
        else if (timer >= 30 && timer < 60)
        {
            tiltPhone[0].enabled = false;
            tiltPhone[1].enabled = true;
            tiltPhone[2].enabled = false;

            tapPhone[0].enabled = false;
            tapPhone[1].enabled = true;
        }
        else if (timer >= 90 && timer < 120)
        {
            tiltPhone[0].enabled = false;
            tiltPhone[1].enabled = false;
            tiltPhone[2].enabled = true;

            tapPhone[0].enabled = false;
            tapPhone[1].enabled = true;
        }
        else
            timer = 0;

        //Precalculate shake value
        float shakeTextZ = Mathf.Sin(Time.time) * shakeAmount;

        //Shake beer delivery text to make it stand out
        deliverBeerText[0].transform.Rotate(0, 0, shakeTextZ);
        deliverBeerText[1].transform.Rotate(0, 0, -shakeTextZ);

        shakeAmount *= -1; //Flip to make the beer shake the other way the next frame

        timer++;
	}
}
