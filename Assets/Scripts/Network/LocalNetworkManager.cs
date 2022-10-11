using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;

public enum NetworkDeviceType
{
    DEFAULT,
    SERVER,
    GAMEMASTER,
    CLIENT
}

public class LocalNetworkManager : MonoBehaviour
{
    [NonSerialized]
    public static LocalNetworkManager Singleton;

    NetworkDeviceType networkDeviceType = NetworkDeviceType.DEFAULT;

    bool PlayerWantSpawn = false;
    bool PlayerIsSpawn = false;

    private void OnGUI()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));

            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
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
            if (Singleton != this)
            {
                Debug.LogError("Multiple instances of Singleton LocalNetworkManager !");
            }
        }
    }

    private void Start()
    {
        ///////////////////// RELEASE /////////////////////
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Debug.LogWarning("(Release mode) You have to choose between Server and GameMaster mode !");
        }
        else if(Application.platform != RuntimePlatform.WindowsEditor)
        {
            Debug.LogWarning("(Release mode) You have to choose Client mode !");

            //StartClient();
        }
        ///////////////////// RELEASE /////////////////////
    }

    private void Update()
    {
        if(IsConnectedToServer() && !IsOnNetwork())
        {
            PlayerIsSpawn = false;

            if (networkDeviceType == NetworkDeviceType.GAMEMASTER)
                StartGameMaster();

            if (networkDeviceType == NetworkDeviceType.CLIENT)
                StartClient();
        }
    }

    static void StartButtons()
    {
        if (GUILayout.Button("Server")) LocalNetworkManager.Singleton.StartServer();
        if (GUILayout.Button("GameMaster")) LocalNetworkManager.Singleton.StartGameMaster();
        if (GUILayout.Button("Client")) LocalNetworkManager.Singleton.StartClient();
    }

    public void LocalPlayerIsSpawned()
    {
        PlayerWantSpawn = false;
        PlayerIsSpawn = true;
    }

    public bool WaitConnectionToServer()
    {
        return PlayerWantSpawn && !PlayerIsSpawn;
    }

    public bool IsConnectedToServer()
    {
        return !PlayerWantSpawn && PlayerIsSpawn;
    }

    public bool IsOnNetwork()
    {
        return NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer;
    }

    public void StartServer()
    {
        PlayerWantSpawn = true;

        networkDeviceType = NetworkDeviceType.SERVER;

        NetworkManager.Singleton.StartServer();
    }

    public void StartGameMaster()
    {
        PlayerWantSpawn = true;

        networkDeviceType = NetworkDeviceType.GAMEMASTER;

        NetworkManager.Singleton.StartClient();
    }

    public void StartClient()
    {
        PlayerWantSpawn = true;

        networkDeviceType = NetworkDeviceType.CLIENT;

        NetworkManager.Singleton.StartClient();
    }

    public NetworkDeviceType GetNetworkDeviceType()
    {
        return networkDeviceType;
    }
}
