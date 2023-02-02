using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enigme : MonoBehaviour
{
   
   private static int nextID=0;
   protected int ID = 0;

    private static int score=0;

    private bool enigmeSolved=false;

    private void Awake()
    {
        ID = nextID++;
        SetScore(0);
    }

    public int getID()
    { 
        return ID;
    }  
    protected void SetSolved()
    {
        enigmeSolved = true;
        SetScore(GetScore() + 1);
        EnigmeManager.instance.goToNext(ID);
    }
    public void SetScore(int sc)
    {
        score = sc;
    }
    public int GetScore()
    {
        return score;
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
