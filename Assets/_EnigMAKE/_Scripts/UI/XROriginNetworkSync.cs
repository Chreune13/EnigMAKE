using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class XROriginNetworkSync : NetworkBehaviour
{
    [SerializeField]
    NetworkVariable<int> clientId = new NetworkVariable<int>() { Value = 0 };
    static int nextClientId = 1;

    [SerializeField]
    public GameObject HeadOffset;

    [Space]

    [SerializeField]
    public GameObject LeftHandOffset;

    [SerializeField]
    public Hand LeftHandAnimatorScript;

    [Space]

    [SerializeField]
    public GameObject RightHandOffset;

    [SerializeField]
    public Hand RightHandAnimatorScript;

    private void Start()
    {
        if(IsClient)
            StartCoroutine(ClientIdRecivedCoroutine());

        if(IsServer)
        {
            clientId.Value = nextClientId++;

            if (PlayerDataSharing.Singleton)
                PlayerDataSharing.Singleton.SyncronizeRemotePlayer(this);
        }
    }

    public override void OnDestroy()
    {
        if (IsOwner)
        {
            if (PlayerDataSharing.Singleton)
                PlayerDataSharing.Singleton.UnsyncronizeLocalPlayer();
        }
        else
        {
            if (PlayerDataSharing.Singleton)
                PlayerDataSharing.Singleton.UnsyncronizeRemotePlayer(this);
        }

        base.OnDestroy();
    }

    public int GetPlayerId()
    {
        return clientId.Value;
    }

    IEnumerator ClientIdRecivedCoroutine()
    {
        while (clientId.Value == 0)
            yield return new WaitForSeconds(0.1f);

        if (IsOwner)
        {
            if (XROriginRoot.Singleton)
                XROriginRoot.Singleton.SetNetworkClone(this);

            if (PlayerDataSharing.Singleton)
                PlayerDataSharing.Singleton.SetLocalPlayer(this);
        }
        else
        {
            if (PlayerDataSharing.Singleton)
                PlayerDataSharing.Singleton.SyncronizeRemotePlayer(this);
        }

        yield return null;
    }
}

