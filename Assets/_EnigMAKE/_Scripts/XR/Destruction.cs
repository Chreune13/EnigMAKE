using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Destruction : MonoBehaviour
{
    void DestructionObject()
    {
        Rigidbody[] rigibObject = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigib in rigibObject)
        {
            rigib.isKinematic = false;
        }
    }


}
