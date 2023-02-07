using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsOnNetwork : MonoBehaviour
{
    public static IsOnNetwork Singleton;

    private void Awake()
    {

        if (Singleton != null)
        {
            Debug.LogError("Multiple instance of singleton IsOnNetwork!");
            gameObject.SetActive(false);
            return;
        }

        Singleton = this;
    }
}
