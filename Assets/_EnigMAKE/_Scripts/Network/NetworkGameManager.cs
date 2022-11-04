using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct PlayerNetworkData
{
    public bool PlayerNetworkDataIsSet;
    public ulong PlayerNetworkId;
    public PlayerType PlayerNetworkType;
}

public class NetworkGameManager : NetworkBehaviour
{
    [NonSerialized]
    public static NetworkGameManager Singleton;

    [NonSerialized]
    public static int MAX_PLAYER = 4;

    PlayerNetworkData LastConnectedPlayer;

    PlayerNetworkData GameMasterNetworkData;

    PlayerNetworkData[] PlayersNetworkData = new PlayerNetworkData[MAX_PLAYER];

    [SerializeField]
    private bool StartAsAServer = false;

    PlayerType localClientType;

    bool PlayerWantSpawn = false;
    bool PlayerIsSpawn = false;

    DisplayedInterface CurrentDisplayed = DisplayedInterface.NOTHING;

    public Action SelectNothingCallback;
    public Action SelectClientTypeAskingInterfaceCallback;
    public Action SelectWaitingServerLogginInterfaceCallback;
    public Action SelectGameMasterInterfaceCallback;
    public Action SelectPlayerInterfaceCallback;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        LastConnectedPlayer.PlayerNetworkDataIsSet = false;
        GameMasterNetworkData.PlayerNetworkDataIsSet = false;

        for(int i = 0; i < MAX_PLAYER; i++)
            PlayersNetworkData[i].PlayerNetworkDataIsSet = false;

        if (Singleton == null)
            Singleton = this;
        else
        {
            Debug.LogError("Multiple instances of Singleton NetworkGameManager !");
        }
    }

    private void Start()
    {
        if(StartAsAServer)
        {
            StartServer();
        }
        else
        {
            if (CurrentDisplayed != DisplayedInterface.CLIENT_TYPE_ASKING)
            {
                CurrentDisplayed = DisplayedInterface.CLIENT_TYPE_ASKING;

                SelectClientTypeAskingInterfaceCallback();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ProcessLastConnectedPlayer();

        CheckLocalPlayerIsConnected();

        UpdateInterfaceStatus();
    }

    private void ProcessLastConnectedPlayer()
    {
        if (!IsServer)
            return;

        if (!LastConnectedPlayer.PlayerNetworkDataIsSet)
            return;

        if (LastConnectedPlayer.PlayerNetworkType == PlayerType.GAMEMASTER)
        {
            if (GameMasterNetworkData.PlayerNetworkDataIsSet)
            {
                Debug.LogWarning("A GameMaster is already connected");

                DisconnectLastConnectedPlayer();
            }
            else
            {
                GameMasterNetworkData = LastConnectedPlayer;
                GameMasterNetworkData.PlayerNetworkDataIsSet = true;
            }
        }
        else if (LastConnectedPlayer.PlayerNetworkType == PlayerType.PLAYER)
        {
            int i;

            for (i = 0; i < MAX_PLAYER; i++)
            {
                if(!PlayersNetworkData[i].PlayerNetworkDataIsSet)
                {
                    PlayersNetworkData[i] = LastConnectedPlayer;
                    PlayersNetworkData[i].PlayerNetworkDataIsSet = true;

                    break;
                }
            }

            if(i == MAX_PLAYER)
            {
                Debug.LogWarning("Cannot connect a new Player, limit reach");

                DisconnectLastConnectedPlayer();
            }
        }

        LastConnectedPlayer.PlayerNetworkDataIsSet = false;
    }

    private void CheckLocalPlayerIsConnected()
    {
        if (IsConnectedToServer() && !IsOnNetwork())
        {
            PlayerIsSpawn = false;


            /*if (networkDeviceType == NetworkDeviceType.GAMEMASTER)
                StartGameMaster();

            if (networkDeviceType == NetworkDeviceType.CLIENT)
                StartClient();*/
        }
    }

    private void UpdateInterfaceStatus()
    {
        if (IsServer)
            return;

        if(!IsConnectedToServer())
        {
            if (WaitConnectionToServer())
            {
                if (CurrentDisplayed != DisplayedInterface.WAITING_SERVER_LOGGIN)
                {
                    CurrentDisplayed = DisplayedInterface.WAITING_SERVER_LOGGIN;

                    SelectWaitingServerLogginInterfaceCallback();
                }
            }
            else
            {
                if (CurrentDisplayed != DisplayedInterface.CLIENT_TYPE_ASKING)
                {
                    CurrentDisplayed = DisplayedInterface.CLIENT_TYPE_ASKING;

                    SelectClientTypeAskingInterfaceCallback();
                }
            }

            return;
        }

        if (NetworkManager.Singleton.LocalClient == null)
            return;

        PlayerType playerType = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerNetworkController>().GetPlayerType();

        if (playerType == PlayerType.GAMEMASTER)
        {
            if (CurrentDisplayed != DisplayedInterface.GAME_MASTER_INTERFACE)
            {
                CurrentDisplayed = DisplayedInterface.GAME_MASTER_INTERFACE;

                SelectGameMasterInterfaceCallback();
            }
            return;
        }

        if (playerType == PlayerType.PLAYER)
        {
            if (CurrentDisplayed != DisplayedInterface.PLAYER_INTERFACE)
            {
                CurrentDisplayed = DisplayedInterface.PLAYER_INTERFACE;

                SelectPlayerInterfaceCallback();
            }
            return;
        }
    }

    public void NewPlayerConnect(ulong playerId, PlayerType ClientType)
    {
        NewPlayerConnectServerRpc(playerId, ClientType);
    }

    private void DisconnectLastConnectedPlayer()
    {
        if (!IsServer)
            return;

        LastConnectedPlayer.PlayerNetworkDataIsSet = false;

        NetworkManager.Singleton.DisconnectClient(LastConnectedPlayer.PlayerNetworkId);
    }

    public void DisconnectConnectedPlayer(ulong playerId, bool orderClientToDisconnect)
    {
        if (!IsServer)
            return;

        if(GameMasterNetworkData.PlayerNetworkDataIsSet)
        {
            if(GameMasterNetworkData.PlayerNetworkId == playerId)
            {
                GameMasterNetworkData.PlayerNetworkDataIsSet = false;

                if(orderClientToDisconnect) NetworkManager.Singleton.DisconnectClient(GameMasterNetworkData.PlayerNetworkId);

                Debug.Log("Gamemaster disconnected");

                return;
            }
        }

        for (int i = 0; i < MAX_PLAYER; i++)
        {
            if (!PlayersNetworkData[i].PlayerNetworkDataIsSet)
                continue;

            if (PlayersNetworkData[i].PlayerNetworkId != playerId)
                continue;

            PlayersNetworkData[i].PlayerNetworkDataIsSet = false;

            if(orderClientToDisconnect) NetworkManager.Singleton.DisconnectClient(PlayersNetworkData[i].PlayerNetworkId);

            Debug.Log("Client disconnected");

            break;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void NewPlayerConnectServerRpc(ulong playerId, PlayerType playerType)
    {
        LastConnectedPlayer.PlayerNetworkDataIsSet = true;
        LastConnectedPlayer.PlayerNetworkId = playerId;
        LastConnectedPlayer.PlayerNetworkType = playerType;
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

    private void StartServer()
    {
        PlayerWantSpawn = true;

        NetworkManager.Singleton.StartServer();
    }

    public void StartGameMaster()
    {
        PlayerWantSpawn = true;

        localClientType = PlayerType.GAMEMASTER;

        NetworkManager.Singleton.StartClient();
    }

    public void StartClient()
    {
        PlayerWantSpawn = true;

        localClientType = PlayerType.PLAYER;

        NetworkManager.Singleton.StartClient();
    }

    public PlayerType GetSelectedClientType()
    {
        return localClientType;
    }
}
