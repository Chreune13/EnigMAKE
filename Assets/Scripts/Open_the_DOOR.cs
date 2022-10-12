using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Open_the_DOOR : Enigme
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag =="Key")
        {
            
            StartCoroutine(Wait(other));
        }
    }

    private IEnumerator Wait(Collider other)
    {
        other.gameObject.GetComponent<Animator>().SetBool("OPEN", true);
        //other.gameObject.transform.SetParent(transform, true);
        yield return new WaitForSeconds(1.5f);
       
        gameObject.GetComponent<XRSocketInteractor>().socketActive = true;
      
        //gameObject.GetComponent<Animator>().SetBool("DoorOpened", true);
        SetSolved();
        
    }
   
}
