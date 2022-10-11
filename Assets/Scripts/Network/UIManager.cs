using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [NonSerialized]
    public static UIManager Singleton;

    [SerializeField]
    GameObject GameMasterOrClientUI;

    //[SerializeField]
    //GameObject GameObject

    private void Awake()
    {
        if(Singleton == null)
            Singleton = this;
        else
        {
            if(Singleton != this)
            {
                Debug.LogError("Multiple instances of Singleton UIManager !");
            }
        }
    }

    public void SetGameMasterOrClientUIState(bool state)
    {
        if(GameMasterOrClientUI != null)
            GameMasterOrClientUI.SetActive(state);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameMasterOrClientUI == null) Debug.LogWarning("GameMasterOrClientUI is not set in UIManager !");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
