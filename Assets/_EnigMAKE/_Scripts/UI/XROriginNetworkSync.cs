using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class XROriginNetworkSync : NetworkBehaviour
{
    [NonSerialized]
    public NetworkVariable<int> lookingId = new NetworkVariable<int> { Value = 0 };

    [SerializeField]
    XROriginRoot localXROriginRoot;

    [SerializeField]
    GameObject Head;
    [SerializeField]
    GameObject LeftHand;
    [SerializeField]
    GameObject RightHand;

    // Start is called before the first frame update
    void Start()
    {
        if (IsClient)
        {
            StartCoroutine(SearchXROriginRootCoroutine());
        }
    }

    private void Update()
    {
        if(localXROriginRoot)
        {
            if (localXROriginRoot.playerId == lookingId.Value)
            {
                if (gameObject && localXROriginRoot.gameObject)
                {
                    SyncBodyTransformServerRpc(localXROriginRoot.gameObject.transform.localPosition, localXROriginRoot.gameObject.transform.localRotation, localXROriginRoot.gameObject.transform.localScale);

                    transform.localPosition = localXROriginRoot.gameObject.transform.localPosition;
                    transform.localRotation = localXROriginRoot.gameObject.transform.localRotation;
                    transform.localScale = localXROriginRoot.gameObject.transform.localScale;
                }

                if (Head && localXROriginRoot.HeadOrigin)
                {
                    SyncHeadTransformServerRpc(localXROriginRoot.HeadOrigin.transform.localPosition, localXROriginRoot.HeadOrigin.transform.localRotation, localXROriginRoot.HeadOrigin.transform.localScale);

                    Head.transform.localPosition = localXROriginRoot.HeadOrigin.transform.localPosition;
                    Head.transform.localRotation = localXROriginRoot.HeadOrigin.transform.localRotation;
                    Head.transform.localScale = localXROriginRoot.HeadOrigin.transform.localScale;
                }

                if (LeftHand && localXROriginRoot.LeftHandOrigin)
                {
                    SyncLeftHandTransformServerRpc(localXROriginRoot.LeftHandOrigin.transform.localPosition, localXROriginRoot.LeftHandOrigin.transform.localRotation, localXROriginRoot.LeftHandOrigin.transform.localScale);

                    LeftHand.transform.localPosition = localXROriginRoot.LeftHandOrigin.transform.localPosition;
                    LeftHand.transform.localRotation = localXROriginRoot.LeftHandOrigin.transform.localRotation;
                    LeftHand.transform.localScale = localXROriginRoot.LeftHandOrigin.transform.localScale;
                }

                if (RightHand && localXROriginRoot.RightHandOrigin)
                {
                    SyncRightHandTransformServerRpc(localXROriginRoot.RightHandOrigin.transform.localPosition, localXROriginRoot.RightHandOrigin.transform.localRotation, localXROriginRoot.RightHandOrigin.transform.localScale);

                    RightHand.transform.localPosition = localXROriginRoot.RightHandOrigin.transform.localPosition;
                    RightHand.transform.localRotation = localXROriginRoot.RightHandOrigin.transform.localRotation;
                    RightHand.transform.localScale = localXROriginRoot.RightHandOrigin.transform.localScale;
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SyncBodyTransformServerRpc(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        transform.localPosition = position;
        transform.localRotation = rotation;
        transform.localScale = scale;
    }

    [ServerRpc(RequireOwnership = false)]
    void SyncHeadTransformServerRpc(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        if(Head)
        {
            Head.transform.localPosition = position;
            Head.transform.localRotation = rotation;
            Head.transform.localScale = scale;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SyncLeftHandTransformServerRpc(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        if(LeftHand)
        {
            LeftHand.transform.localPosition = position;
            LeftHand.transform.localRotation = rotation;
            LeftHand.transform.localScale = scale;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SyncRightHandTransformServerRpc(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        if (RightHand)
        {
            RightHand.transform.localPosition = position;
            RightHand.transform.localRotation = rotation;
            RightHand.transform.localScale = scale;
        }
    }

    IEnumerator SearchXROriginRootCoroutine()
    {
        while (lookingId.Value == 0)
            yield return new WaitForSeconds(0.1f);

        while(FindObjectsOfType<XROriginRoot>().Length == 0)
            yield return new WaitForSeconds(0.1f);

        foreach(XROriginRoot xROriginRoot in FindObjectsOfType<XROriginRoot>())
        {
            if(xROriginRoot.playerId == lookingId.Value)
            {
                localXROriginRoot = xROriginRoot;

                GetComponent<TransformNetworkSync>().enabled = false;
                if (Head) Head.GetComponent<TransformNetworkSync>().enabled = false;
                if (LeftHand) LeftHand.GetComponent<TransformNetworkSync>().enabled = false;
                if (RightHand) RightHand.GetComponent<TransformNetworkSync>().enabled = false;

                break;
            }
        }

        yield return null;
    }
}
