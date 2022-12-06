using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Enigme
{
    override protected void ExecuteAction1()
    {

        GetComponent<Animator>().SetBool("ChestOpen", true);
        SetSolved();

    }
}
