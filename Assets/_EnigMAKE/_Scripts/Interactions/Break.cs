using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : Enigme
{
    override protected void ExecuteAction1()
    {
        Rigidbody[] rigibObject = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigib in rigibObject)
        {
            rigib.isKinematic = false;
        }
        SetSolved();
    }
}
