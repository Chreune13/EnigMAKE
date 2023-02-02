using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Enigme
{
    override protected void ExecuteAction1()
    {
        
        GetComponent<Animator>().SetBool("DoorOpened", true);
        SetSolved();
        SetScore(GetScore() + 1);
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    other.gameObject.transform.SetParent(transform, true);
    //}
    
}
