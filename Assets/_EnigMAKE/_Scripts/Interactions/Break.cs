using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : Enigme
{

    override public void ExecuteAction()
    {
        Rigidbody[] rigibObject = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigib in rigibObject)
        {
            rigib.isKinematic = false;
        }
        //SetSolved();
        
    }

   
}
