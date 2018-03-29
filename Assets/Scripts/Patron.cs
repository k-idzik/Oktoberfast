using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patron : MonoBehaviour {

    public enum Patrons { JOEL = 0, JOSH = 1, JOHN = 2, UNKNOWN =3};

    //Patron Attributes
    [SerializeField] private int patronNum = 0;  //What number Patron is this in the level
    [SerializeField] private Patrons patronId;     //Which Patron is this? (Josh, Joel, or John)
    //[SerializeField] private Patrons nextPatron; //Which Patron should player Serve Next

    //Patron Getters
    public int PatronNum { get { return patronNum; } }          //Returns the Patron Id Number
    public Patrons PatronId { get { return patronId; } }            //Return who this Patron is
    //public Patrons NextPatron { get { return nextPatron; } }    //Return Next Patron that player should serve
}
