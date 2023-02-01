using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Deplacer3Tuto : MonoBehaviour
{
    //[SerializeField] GameObject FinPainting;
    //[SerializeField] GameObject TeleportationArea;
    //[SerializeField] GameObject DEPLACERGroup;
    //[SerializeField] GameObject Table;
    //[SerializeField] GameObject UI;
    //[SerializeField] GameObject Affiche3;

    [SerializeField] GameObject[] Display;
    [SerializeField] GameObject[] Hide;

    [SerializeField] AudioSource validateSound;

   // [SerializeField] GameObject[] gameobjects;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hand")
        {

            StartCoroutine(Wait(other.gameObject));
        }
        validateSound.Play();
    }
    IEnumerator Wait(GameObject other)
    {
        yield return new WaitForSeconds(1f);

        //FinPainting.SetActive(true);
        //TeleportationArea.SetActive(true);
        //Table.SetActive(true);
        //UI.SetActive(true);

        //Affiche3.SetActive(false);
        //DEPLACERGroup.SetActive(false);

        foreach (var item in Display)
        {
            item.SetActive(true);
        }

        foreach (var item in Hide)
        {
            item.SetActive(false);
        }

    }
}
