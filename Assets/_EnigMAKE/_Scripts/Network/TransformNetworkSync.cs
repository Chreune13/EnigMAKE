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
        if (IsOwner)
        {
            interactable.interactionLayers = LayerMask.NameToLayer("Grabbable");

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
            interactable.interactionLayers = LayerMask.NameToLayer("NotGrabbable");

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
        GetComponent<NetworkObject>().ChangeOwnership(newOwnerId);
    }

    public void ResetOwner()
    {
        ResetOwnerServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ResetOwnerServerRpc()
    {
        GetComponent<NetworkObject>().RemoveOwnership();
    }
}
