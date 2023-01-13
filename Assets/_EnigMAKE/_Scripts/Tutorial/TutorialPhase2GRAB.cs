using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPhase2GRAB : MonoBehaviour
{
    [SerializeField] GameObject[] Desactivate;
    [SerializeField] GameObject[] Activate;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tutorial2Grab")
        {
            StartCoroutine(Wait());
        }
    }
    
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);

        foreach (GameObject obj in Desactivate)
        {
            obj.SetActive(false);
        }
        
        foreach (GameObject obj in Activate)
        {
            obj.SetActive(true);
        }
    }
}
