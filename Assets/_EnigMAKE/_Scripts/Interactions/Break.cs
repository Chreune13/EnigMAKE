using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : Enigme
{

    //private void Start()
    //{
    //    GetComponent<MeshCollider>().enabled = false;
    //    //GetComponent<BoxCollider>().enabled = true;
    //}

    override protected void ExecuteAction1()
    {
        Rigidbody[] rigibObject = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigib in rigibObject)
        {
            rigib.isKinematic = false;
        }
        SetSolved();
    }

    //private void OnCollideEnter(Collision collision)
    //{
    //    GetComponent<MeshCollider>().enabled = true;
    //    GetComponent<BoxCollider>().enabled = false;
       
    //}
}
