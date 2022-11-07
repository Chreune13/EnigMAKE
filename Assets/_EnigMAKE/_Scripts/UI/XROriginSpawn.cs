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
            if(LocalPlayerPrefab)
            {
                GameObject localPlayer = Instantiate(LocalPlayerPrefab);

                localPlayer.transform.parent = transform;
            }

            //todo network spawn
        }
    }
}
