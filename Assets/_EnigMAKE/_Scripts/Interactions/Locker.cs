using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Locker: Enigme
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag =="Key")
        {
            StartCoroutine(Unlock(other));

        }
    }

    private IEnumerator Unlock(Collider other)
    {
        other.gameObject.GetComponent<Animator>().SetBool("OPEN", true);
        
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<XRSocketInteractor>().socketActive = true;
      
        
        SetSolved();
    }

  
}
