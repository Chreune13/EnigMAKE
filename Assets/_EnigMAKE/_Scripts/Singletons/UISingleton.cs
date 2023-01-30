using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISingleton : MonoBehaviour
{
    static UISingleton instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            gameObject.SetActive(false);
    }
}
