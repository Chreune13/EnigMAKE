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
    [NonSerialized]
    public PlayerType clientType;

    // Start is called before the first frame update
    void Start()
    {
        if (IsOwner && IsClient)
        {
            clientType = LocalNetworkManager.Singleton.GetSelectedClientTypeType();

            LocalNetworkManager.Singleton.LocalPlayerIsSpawned();
            NetworkGameManager.Singleton.NewPlayerConnect(OwnerClientId, clientType);
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    
}
