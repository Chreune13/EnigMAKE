using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class filling : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag !="Untagged")
            other.gameObject.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Untagged")
            other.gameObject.transform.SetParent(null);
    }
}
