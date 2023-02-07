using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CheckIsOnNetwork : MonoBehaviour
{
    void Awake()
    {
        if(IsOnNetwork.Singleton == null)
        {
            if(GetComponent<ShareGrabable>())
                Destroy(GetComponent<ShareGrabable>());

            if(GetComponent<TransformNetworkSync>())
                Destroy(GetComponent<TransformNetworkSync>());

            if(GetComponent<NetworkObject>())
                Destroy(GetComponent<NetworkObject>());
        }
        else
        {
            if(GetComponent<AutoRegisterSave>())
                Destroy(GetComponent<AutoRegisterSave>());

            if(GetComponent<Enigme>())
                Destroy(GetComponent<Enigme>());
        }
    }
}
