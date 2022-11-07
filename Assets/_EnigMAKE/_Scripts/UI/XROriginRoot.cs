using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class XROriginRoot : MonoBehaviour
{
    public int playerId = 0;

    public GameObject HeadOrigin;

    public GameObject LeftHandOrigin;

    public GameObject RightHandOrigin;

    private void OnDestroy()
    {
        foreach(XROriginNetworkSync xROriginNetworkSync in FindObjectsOfType<XROriginNetworkSync>())
        {
            if(xROriginNetworkSync.lookingId.Value == playerId)
            {
                xROriginNetworkSync.gameObject.GetComponent<NetworkObject>().Despawn();
                break;
            }
        }
    }
}
