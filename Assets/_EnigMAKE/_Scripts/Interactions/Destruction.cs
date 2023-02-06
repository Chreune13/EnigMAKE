using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Destruction : Enigme
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Destruction")
        {
            SetSolved();
        }

    }
}