using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum PlayerType
{
    GAMEMASTER,
    PLAYER
}

public class PlayerNetworkController : NetworkBehaviour
{
    NetworkVariable<ulong> playerId = new NetworkVariable<ulong>();

    PlayerType playerType;

    // Start is called before the first frame update
    void Start()
    {
        if (IsOwner && IsClient)
        {
            SyncPlayerIdServerRpc(OwnerClientId);
            playerType = LocalNetworkManager.Singleton.GetSelectedClientTypeType();

            LocalNetworkManager.Singleton.LocalPlayerIsSpawned();
            NetworkGameManager.Singleton.NewPlayerConnect(OwnerClientId, playerType);
        }
    }

    public override void OnDestroy()
    {
        if (IsServer)
        {
            NetworkGameManager.Singleton.DisconnectPlayer(playerId.Value, true);
        }
        
        base.OnDestroy();
    }

    [ServerRpc]
    void SyncPlayerIdServerRpc(ulong p_playerId)
    {
        playerId.Value = p_playerId;
    }

    public ulong GetPlayerId()
    {
        return playerId.Value;
    }

    public PlayerType GetPlayerType()
    {
        return playerType;
    }
}
