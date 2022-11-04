using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DisableComponentsOnOthers : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!IsOwner)
        {
            MonoBehaviour[] components = GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour component in components)
            {
                if (component.GetType() == typeof(DisableComponentsOnOthers))
                    continue;

                if (component.GetType() == typeof(Unity.Netcode.NetworkObject))
                    continue;

                if (component.GetType() == typeof(TransformNetworkSync))
                    continue;

                if (component.GetType() == typeof(PlayerNetworkController))
                    continue;

                if (component.GetType() == typeof(CameraPlayerManagment))
                    continue;

                component.enabled = false;
            }
        }
    }
}
