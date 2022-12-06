using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TransformNetworkSync : NetworkBehaviour
{
    NetworkVariable<Vector3> NetworkPosition = new NetworkVariable<Vector3>();
    NetworkVariable<Quaternion> NetworkRotation = new NetworkVariable<Quaternion>();
    NetworkVariable<Vector3> NetworkScale = new NetworkVariable<Vector3>();

    // Update is called once per frame
    void Update()
    {
        UpdateTransform();
    }

    void UpdateTransform()
    {
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
}
