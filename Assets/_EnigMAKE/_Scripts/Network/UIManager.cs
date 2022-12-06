using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public enum DisplayedInterface
{
    NOTHING,
    CLIENT_TYPE_ASKING,
    WAITING_SERVER_LOGGIN,
    GAME_MASTER_INTERFACE,
    PLAYER_INTERFACE
}

[System.Serializable]
struct networkManagement
{
    public UnityEvent invokeDefaultMethod;
    public GameObject[] toDestroy;
}


public class UIManager : MonoBehaviour
{
    [NonSerialized]
    public static UIManager Singleton;

    [SerializeField]
    private networkManagement[] networkManagement;

    DisplayedInterface CurrentDisplayed = DisplayedInterface.NOTHING;

    private void OnGUI()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));

            switch(CurrentDisplayed)
            {
                case DisplayedInterface.CLIENT_TYPE_ASKING:
                    DisplayClientTypeAskingInterface();
                    break;
                case DisplayedInterface.WAITING_SERVER_LOGGIN:
                    DisplayWaitingServerLogginInterface();
                    break;
                case DisplayedInterface.GAME_MASTER_INTERFACE:
                    DisplayGameMasterInterface();
                    break;
                case DisplayedInterface.PLAYER_INTERFACE:
                    DisplayPlayerInterface();
                    break;
            }

            GUILayout.EndArea();
        }
    }

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
        {
            Debug.LogError("Multiple instances of Singleton UIManager !");
        }

        NetworkGameManager.Singleton.SelectClientTypeAskingInterfaceCallback += SelectClientTypeAskingInterface;
        NetworkGameManager.Singleton.SelectWaitingServerLogginInterfaceCallback += SelectWaitingServerLogginInterface;
        NetworkGameManager.Singleton.SelectGameMasterInterfaceCallback += SelectGameMasterInterface;
        NetworkGameManager.Singleton.SelectPlayerInterfaceCallback += SelectPlayerInterface;
    }

    private void OnDestroy()
    {
        NetworkGameManager.Singleton.SelectClientTypeAskingInterfaceCallback -= SelectClientTypeAskingInterface;
        NetworkGameManager.Singleton.SelectWaitingServerLogginInterfaceCallback -= SelectWaitingServerLogginInterface;
        NetworkGameManager.Singleton.SelectGameMasterInterfaceCallback -= SelectGameMasterInterface;
        NetworkGameManager.Singleton.SelectPlayerInterfaceCallback -= SelectPlayerInterface;
    }



    //-------------------------- CLIENT TYPE -------------------------

    public void SelectClientTypeAskingInterface()
    {
        CurrentDisplayed = DisplayedInterface.CLIENT_TYPE_ASKING;
    }

    void DisplayClientTypeAskingInterface()
    {
        if (GUILayout.Button("GameMaster")) GameMaster();
        if (GUILayout.Button("Client")) Client();
    }

    public void GameMaster()
    {
        //NetworkGameManager.Singleton.StartGameMaster();
    }

    public void Client()
    {
        NetworkGameManager.Singleton.StartClient();
    }


    //-------------------------- WAITING SERVER -------------------------
    public void SelectWaitingServerLogginInterface()
    {
        CurrentDisplayed = DisplayedInterface.WAITING_SERVER_LOGGIN;
    }

    void DisplayWaitingServerLogginInterface()
    {
        GUILayout.Label("En attente de connexion au serveur");
    }




    //-------------------------- GAME MASTER CLIENT -------------------------
    public void SelectGameMasterInterface()
    {
        CurrentDisplayed = DisplayedInterface.GAME_MASTER_INTERFACE;
        Debug.Log("GameMaster Interface");
    }

    void DisplayGameMasterInterface()
    {
        GUILayout.Label("GameMaster Interface");
    }



    //-------------------------- PLAYER CLIENT -------------------------
    public void SelectPlayerInterface()
    {
        CurrentDisplayed = DisplayedInterface.PLAYER_INTERFACE;
    }

    void DisplayPlayerInterface()
    {
        GUILayout.Label("Player en attente du GameMaster");
        Debug.Log("Player en attente du GameMaster");

        foreach (networkManagement item in networkManagement)
        {
            foreach (var objDestroy in item.toDestroy)
            {
                Destroy(objDestroy);
            }
           
        }
    }
}
