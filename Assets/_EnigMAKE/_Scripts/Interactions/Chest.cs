using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Enigme
{
    override public void ExecuteAction()
    {

        GetComponent<Animator>().SetBool("ChestOpen", true);
        //SetSolved();
        
    }
}
