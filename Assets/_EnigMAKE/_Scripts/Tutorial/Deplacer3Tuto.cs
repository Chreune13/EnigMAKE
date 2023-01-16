using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Deplacer3Tuto : MonoBehaviour
{
    [SerializeField] GameObject FinPainting;
    [SerializeField] GameObject TeleportationArea;
    [SerializeField] GameObject DEPLACERGroup;
    [SerializeField] GameObject Table;
    [SerializeField] GameObject UI;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hand")
        {

            StartCoroutine(Wait(other.gameObject));
        }
    }

    IEnumerator Wait(GameObject other)
    {
        yield return new WaitForSeconds(1f);

        FinPainting.SetActive(true);
        TeleportationArea.SetActive(true);
        Table.SetActive(true);
        UI.SetActive(true);
        DEPLACERGroup.SetActive(false);

    }
}
