using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enigme : MonoBehaviour
{
   
   private static int nextID=0;
   public int ID = 0;

    private bool enigmeSolved=false;

    private void Awake()
    {
        ID = nextID++;
    }
    protected void SetSolved()
    {
        enigmeSolved = true;

        EnigmeManager.instance.goToNext(ID);
    }    
    public bool isSolved()
    {
        return enigmeSolved; 
    }

    public void ExecuteAction(int actionId)
    {
        switch (actionId)
        {
            case 1:
                ExecuteAction1();
                break;
            case 2:
                ExecuteAction2();
                break;
            case 3:
                ExecuteAction3();
                break;
            default:
                break;
        }
    }

    virtual protected void ExecuteAction1()
    {

    }

    virtual protected void ExecuteAction2()
    {

    }

    virtual protected void ExecuteAction3()
    {

    }
}
