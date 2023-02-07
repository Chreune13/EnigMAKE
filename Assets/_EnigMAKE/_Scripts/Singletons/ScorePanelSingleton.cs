using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePanelSingleton : MonoBehaviour
{
    public static ScorePanelSingleton instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            gameObject.SetActive(false);
    }
}
