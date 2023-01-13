using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialPhase3MOVE : MonoBehaviour
{

    [SerializeField] string sceneNAME;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tutorial2Grab")
        {
            Debug.Log("zz");
            StartCoroutine(Wait(other.gameObject));
        }
     
    }

    IEnumerator Wait(GameObject other)
    {
        yield return new WaitForSeconds(2f);
        
        SceneManager.LoadScene(sceneNAME);

        Destroy(other);

    }
}
