using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enigme : MonoBehaviour
{
   private bool enigmeSolved=false;
    protected void SetSolved()
    {
        enigmeSolved = true;
    }    
    public bool isSolved()
    {
        return enigmeSolved; 
    }
}
