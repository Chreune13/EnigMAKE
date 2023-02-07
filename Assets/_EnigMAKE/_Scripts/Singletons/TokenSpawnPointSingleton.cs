using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenSpawnPointSingleton : MonoBehaviour
{
    public static TokenSpawnPointSingleton instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            gameObject.SetActive(false);
    }
}
