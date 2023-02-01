using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class ShareGrabable : NetworkBehaviour
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
        ChangeOwnerServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeOwnerServerRpc(ulong newOwnerId)
    {
        GetComponent<NetworkObject>().ChangeOwnership(newOwnerId);

        ChangeOwnerClientRpc(newOwnerId);
    }

    [ClientRpc]
    void ChangeOwnerClientRpc(ulong newOwnerId)
    {
        if(newOwnerId != NetworkManager.Singleton.LocalClientId)
        {
            GetComponent<XRGrabInteractable>().enabled = false;
        }
    }

    void ObjectIsNotGrabbed(SelectExitEventArgs args)
    {
        ResetOwnerServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ResetOwnerServerRpc()
    {
        ResetOwnerClientRpc();
    }

    [ClientRpc]
    void ResetOwnerClientRpc()
    {
        GetComponent<XRGrabInteractable>().enabled = true;
    }
}
