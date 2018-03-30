using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyby : MonoBehaviour
{
    [SerializeField] private GameObject[] instructions = new GameObject[4];
    [SerializeField] private UnityEngine.UI.Text advance;
    private int currentInstruction = 0;

    private MenuManager mMan;

	//Use this for initialization
	void Start()
    {
        mMan = GameObject.Find("MenuManager").GetComponent<MenuManager>(); //Get the GameManager
    }

    //Fixed updates
    void FixedUpdate()
    {
#if UNITY_EDITOR
        mMan.GoToScene(1); //Start game
#endif

        //Zoom in the instruction
        if (instructions[currentInstruction].transform.localScale.z < 1)
        {
            instructions[currentInstruction].transform.localScale += new Vector3(.05f, .05f, .05f);
        }

        //Advance on screen touch
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            instructions[currentInstruction].transform.localScale = Vector3.zero; //Zip out that old instruction

            currentInstruction++; //Next instruction

            //After last instruction
            if (currentInstruction == 4)
                mMan.GoToScene(1); //Start game
            else
                advance.text = "Tap to advance (" + (currentInstruction + 1) + "/4)"; //Update the text
        }
    }
}
