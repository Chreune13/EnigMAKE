using System;
using System.Collections;
using System.Collections.Generic;
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

public class UIManager : MonoBehaviour
{
    [NonSerialized]
    public static UIManager Singleton;

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

    public void SelectClientTypeAskingInterface()
    {
        CurrentDisplayed = DisplayedInterface.CLIENT_TYPE_ASKING;
    }

    void DisplayClientTypeAskingInterface()
    {
        if (GUILayout.Button("GameMaster")) NetworkGameManager.Singleton.StartGameMaster();
        if (GUILayout.Button("Client")) NetworkGameManager.Singleton.StartClient();
    }

    public void SelectWaitingServerLogginInterface()
    {
        CurrentDisplayed = DisplayedInterface.WAITING_SERVER_LOGGIN;
    }

    void DisplayWaitingServerLogginInterface()
    {
        GUILayout.Label("En attente de connexion au serveur");
    }

    public void SelectGameMasterInterface()
    {
        CurrentDisplayed = DisplayedInterface.GAME_MASTER_INTERFACE;
        Debug.Log("GameMaster Interface");
    }

    void DisplayGameMasterInterface()
    {
        GUILayout.Label("GameMaster Interface");
    }

    public void SelectPlayerInterface()
    {
        CurrentDisplayed = DisplayedInterface.PLAYER_INTERFACE;
    }

    void DisplayPlayerInterface()
    {
        GUILayout.Label("Player en attente du GameMaster");
    }
}
