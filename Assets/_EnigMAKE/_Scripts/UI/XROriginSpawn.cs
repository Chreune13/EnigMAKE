using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class XROriginSpawn : NetworkBehaviour
{
    [SerializeField]
    GameObject LocalPlayerPrefab;

    [SerializeField]
    GameObject NetworkPlayerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if(IsOwner)
        {
            int id = UnityEngine.Random.Range(1, 2000000);

            if(LocalPlayerPrefab)
            {
                GameObject localPlayer = Instantiate(LocalPlayerPrefab);

                localPlayer.GetComponent<XROriginRoot>().playerId = id;

                localPlayer.transform.parent = transform;

                localPlayer.transform.localPosition = Vector3.zero;
            }

            SpawnNetworkPlayerServerRpc(id);
        }
    }

    [ServerRpc]
    void SpawnNetworkPlayerServerRpc(int id)
    {
        if (NetworkPlayerPrefab)
        {
            GameObject remotePlayer = Instantiate(NetworkPlayerPrefab);

            remotePlayer.GetComponent<XROriginNetworkSync>().lookingId.Value = id;

            remotePlayer.GetComponent<NetworkObject>().Spawn();
        }
    }
}
