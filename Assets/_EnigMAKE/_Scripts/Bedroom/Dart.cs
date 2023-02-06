using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Dart : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "DartBoard")
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            //Destroy(gameObject.GetComponent<XRGrabInteractable>());
            //Destroy(gameObject.GetComponent<Rigidbody>());
            //Destroy(gameObject.GetComponent<BoxCollider>());

        }
    }

}

