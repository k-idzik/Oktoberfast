using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patron : MonoBehaviour {

    public enum Patrons { JOEL = 0, JOSH = 1, JOHN = 2, UNKNOWN =3};

    //Patron Attributes
    [SerializeField] private int patronNum = 0;  //What number Patron is this in the level
    [SerializeField] private Patrons patronId;     //Which Patron is this? (Josh, Joel, or John)
    private bool currentPatron = false; //Is this Patron the current patron target for the Player
    private SpriteRenderer spriteRenderer;
    private Camera mainCam;
    

    //Patron Getters
    public int PatronNum { get { return patronNum; } }          //Returns the Patron Id Number
    public Patrons PatronId { get { return patronId; } }            //Return who this Patron is
    public bool CurrentPatron
    {
        get { return currentPatron; }
        set
        {
            currentPatron = value;
        }
    }
    public SpriteRenderer Renderer { get { return spriteRenderer; } }


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        transform.LookAt(mainCam.transform.position, Vector3.up);
    }
}
