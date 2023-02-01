using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManagerSingleton : MonoBehaviour
{
    public static NetworkManagerSingleton instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            gameObject.SetActive(false);
    }
}
