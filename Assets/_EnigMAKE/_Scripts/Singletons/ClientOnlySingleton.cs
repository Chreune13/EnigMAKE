using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientOnlySingleton : MonoBehaviour
{
    static ClientOnlySingleton instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            gameObject.SetActive(false);
    }
}
