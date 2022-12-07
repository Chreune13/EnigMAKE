using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class TransformNetworkSync : NetworkBehaviour
{
    NetworkVariable<Vector3> NetworkPosition = new NetworkVariable<Vector3>();
    NetworkVariable<Quaternion> NetworkRotation = new NetworkVariable<Quaternion>();
    NetworkVariable<Vector3> NetworkScale = new NetworkVariable<Vector3>();

    XRGrabInteractable interactable;

    NetworkVariable<ulong> ownerId = new NetworkVariable<ulong>(0);

    private void Awake()
    {
        interactable = GetComponent<XRGrabInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTransform();
    }

    void UpdateTransform()
    {
        if(ownerId.Value == 0 || ownerId.Value == NetworkManager.Singleton.LocalClientId)
        {
            string[] layerMaskNames = { "Grabbable" };
            interactable.interactionLayers = InteractionLayerMask.GetMask(layerMaskNames);
        }
        else
        {
            string[] layerMaskNames = { "NotGrabbable" };
            interactable.interactionLayers = InteractionLayerMask.GetMask(layerMaskNames);
        }

        if (IsOwner)
        {
            if (IsServer)
            {
                NetworkPosition.Value = transform.localPosition;
                NetworkRotation.Value = transform.localRotation;
                NetworkScale.Value = transform.localScale;
            }
            else
            {
                PositionSyncServerRpc(transform.localPosition);
                RotationSyncServerRpc(transform.localRotation);
                ScaleSyncServerRpc(transform.localScale);
            }
        }
        else
        {
            transform.localPosition = NetworkPosition.Value;
            transform.localRotation = NetworkRotation.Value;
            transform.localScale = NetworkScale.Value;
        }
    }

    [ServerRpc]
    private void PositionSyncServerRpc(Vector3 position)
    {
        NetworkPosition.Value = position;
    }

    [ServerRpc]
    private void RotationSyncServerRpc(Quaternion rotation)
    {
        NetworkRotation.Value = rotation;
    }

    [ServerRpc]
    private void ScaleSyncServerRpc(Vector3 scale)
    {
        NetworkScale.Value = scale;
    }

    public void ChangeOwner(ulong newOwnerId)
    {
        ChangeOwnerServerRpc(newOwnerId);
    }

    [ServerRpc(RequireOwnership=false)]
    private void ChangeOwnerServerRpc(ulong newOwnerId)
    {
        if(ownerId.Value == 0)
        {
            ownerId.Value = newOwnerId;
            GetComponent<NetworkObject>().ChangeOwnership(newOwnerId);

            Debug.Log("Grab by " + newOwnerId);
        }
    }

    public void ResetOwner(ulong oldOwnerId)
    {
        ResetOwnerServerRpc(oldOwnerId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ResetOwnerServerRpc(ulong oldOwnerId)
    {
        if(ownerId.Value == oldOwnerId)
        {
            ownerId.Value = 0;
            GetComponent<NetworkObject>().RemoveOwnership();

            Debug.Log("Not More Grab");
        }
    }
}
