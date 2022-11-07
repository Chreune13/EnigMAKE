using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DisableComponentsOnOthers : NetworkBehaviour
{

    private void Awake()
    {
        MonoBehaviour[] components = GetComponentsInChildren<MonoBehaviour>();

        foreach (MonoBehaviour component in components)
        {
            if(component.GetType() == typeof(UnityEngine.XR.Interaction.Toolkit.ActionBasedController))
                component.enabled = false;
        }
    }

    private void Start()
    {
        MonoBehaviour[] components = GetComponentsInChildren<MonoBehaviour>();

        foreach (MonoBehaviour component in components)
        {
            if (!IsOwner)
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
            else
            {
                if (component.GetType() == typeof(UnityEngine.XR.Interaction.Toolkit.ActionBasedController))
                    component.enabled = true;
            }
        }
    }
}
