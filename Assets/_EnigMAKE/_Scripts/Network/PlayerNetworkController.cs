using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class PlayerNetworkController : NetworkBehaviour
{
    NetworkVariable<ulong> playerId = new NetworkVariable<ulong>();

    // Start is called before the first frame update
    void Start()
    {
        if (IsOwner && IsClient)
        {
            SyncPlayerIdServerRpc(OwnerClientId);

            NetworkGameManager.Singleton.NewPlayerConnect(OwnerClientId);
        }
    }

    public override void OnDestroy()
    {
        if (IsServer)
        {
            NetworkGameManager.Singleton.DisconnectConnectedPlayer(playerId.Value);
        }
        
        base.OnDestroy();
    }

    [ServerRpc]
    void SyncPlayerIdServerRpc(ulong p_playerId)
    {
        playerId.Value = p_playerId;
    }
}
