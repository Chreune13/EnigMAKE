using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class ShareGrabable : MonoBehaviour
{
    XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(ObjectIsGrabbed);
        grabInteractable.selectExited.AddListener(ObjectIsNotGrabbed);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveAllListeners();
        grabInteractable.selectExited.RemoveAllListeners();
    }

    public void ObjectIsGrabbed(SelectEnterEventArgs args)
    {
        TransformNetworkSync netSync = GetComponent<TransformNetworkSync>();

        if(netSync != null)
        {
            netSync.ChangeOwner(NetworkManager.Singleton.LocalClientId);

            Debug.Log("Grab");
        }
    }

    void ObjectIsNotGrabbed(SelectExitEventArgs args)
    {
        TransformNetworkSync netSync = GetComponent<TransformNetworkSync>();

        if (netSync != null)
        {
            netSync.ResetOwner();

            Debug.Log("Not Grab");
        }
    }
}
