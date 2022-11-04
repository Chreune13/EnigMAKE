using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Destruction : MonoBehaviour
{
    void DestructionObject(GameObject p_object)
    {
        Rigidbody[] rigibObject = p_object.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigib in rigibObject)
        {
            rigib.isKinematic = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Destruction")
        {
            DestructionObject(collision.gameObject);
        }

    }
}