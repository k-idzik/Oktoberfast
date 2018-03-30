using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScreenAnimation : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image[] tiltPhone = new UnityEngine.UI.Image[3]; //Mobile tilt images
    int timer = 0;

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
        }
        else if (timer >= 30 && timer < 60)
        {
            tiltPhone[0].enabled = false;
            tiltPhone[1].enabled = true;
            tiltPhone[2].enabled = false;
        }
        else if (timer >= 90 && timer < 120)
        {
            tiltPhone[0].enabled = false;
            tiltPhone[1].enabled = false;
            tiltPhone[2].enabled = true;
        }
        else
            timer = 0;

        timer++;
	}
}
