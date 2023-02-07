using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Enigme
{
    override public void ExecuteAction()
    {
        
        GetComponent<Animator>().SetBool("DoorOpened", true);
        //SetSolved();
        
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    other.gameObject.transform.SetParent(transform, true);
    //}
    
}
