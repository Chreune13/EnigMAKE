using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enigme : MonoBehaviour
{
   
   private static int nextID=0;
   protected int ID = 0;

    private bool enigmeSolved=false;

    private void Awake()
    {
        ID = nextID++;
    }

    public int getID()
    { 
        return ID;
    }  
    protected void SetSolved()
    {
        if (enigmeSolved)
            return;

        enigmeSolved = true;
        EnigmeManager.instance.IncrementScore();
        EnigmeManager.instance.goToNext(ID);
    }

   
    public bool isSolved()
    {
        return enigmeSolved; 
    }

    virtual public void ExecuteAction()
    {
        
    }
}
