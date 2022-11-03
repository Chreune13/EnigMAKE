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

    public static int MAX_PLAYER = 1;

    PlayerNetworkData LastConnectedPlayer;

    PlayerNetworkData GameMasterNetworkData;

    PlayerNetworkData[] PlayersNetworkData = new PlayerNetworkData[MAX_PLAYER];

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
            if (Singleton != this)
            {
                Debug.LogError("Multiple instances of Singleton NetworkGameManager !");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ProcessLastConnectedPlayer();
    }

    void ProcessLastConnectedPlayer()
    {
        if (!IsServer)
            return;

        if (!LastConnectedPlayer.PlayerNetworkDataIsSet)
            return;

        if (LastConnectedPlayer.PlayerNetworkType == PlayerType.GAMEMASTER)
        {
            if (GameMasterNetworkData.PlayerNetworkDataIsSet)
            {
                Debug.Log("A GameMaster is already connected");

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
                Debug.Log("Cannot connect a new Player, limit reach");

                DisconnectLastConnectedPlayer();
            }
        }

        LastConnectedPlayer.PlayerNetworkDataIsSet = false;
    }

    public void NewPlayerConnect(ulong playerId, PlayerType ClientType)
    {
        NewPlayerConnectServerRpc(playerId, ClientType);
    }

    void DisconnectLastConnectedPlayer()
    {
        if (!IsServer)
            return;

        LastConnectedPlayer.PlayerNetworkDataIsSet = false;

        NetworkManager.Singleton.DisconnectClient(LastConnectedPlayer.PlayerNetworkId);
    }

    public void DisconnectPlayer(ulong playerId, bool alreadyOnDestroy = false)
    {
        if (!IsServer)
            return;

        if(GameMasterNetworkData.PlayerNetworkDataIsSet)
        {
            if(GameMasterNetworkData.PlayerNetworkId == playerId)
            {
                GameMasterNetworkData.PlayerNetworkDataIsSet = false;

                if(!alreadyOnDestroy) NetworkManager.Singleton.DisconnectClient(GameMasterNetworkData.PlayerNetworkId);
            }
        }

        for (int i = 0; i < MAX_PLAYER; i++)
        {
            if (!PlayersNetworkData[i].PlayerNetworkDataIsSet)
                continue;

            if (PlayersNetworkData[i].PlayerNetworkId != playerId)
                continue;

            PlayersNetworkData[i].PlayerNetworkDataIsSet = false;

            if(!alreadyOnDestroy) NetworkManager.Singleton.DisconnectClient(PlayersNetworkData[i].PlayerNetworkId);

            Debug.Log("Client disconnected");

            break;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void NewPlayerConnectServerRpc(ulong playerId, PlayerType playerType)
    {
        LastConnectedPlayer.PlayerNetworkDataIsSet = true;
        LastConnectedPlayer.PlayerNetworkId = playerId;
        LastConnectedPlayer.PlayerNetworkType = playerType;
    }
}
