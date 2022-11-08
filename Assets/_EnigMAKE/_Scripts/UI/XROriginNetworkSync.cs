using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.PlayerLoop;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XROriginNetworkSync : NetworkBehaviour
{
    public NetworkVariable<int> lookingId = new NetworkVariable<int> { Value = 0 };

    [SerializeField]
    XROriginRoot localXROriginRoot;

    [Space]

    [SerializeField]
    GameObject HeadOffset;

    [Space]

    [SerializeField]
    GameObject LeftHandOffset;

    ActionBasedController LeftHandActionBasedController;

    [SerializeField]
    GameObject LeftHandAnimator;

    Hand LeftHandAnimatorScript;

    [Space]

    [SerializeField]
    GameObject RightHandOffset;

    ActionBasedController RightHandActionBasedController;

    [SerializeField]
    GameObject RightHandAnimator;

    Hand RightHandAnimatorScript;

    // Start is called before the first frame update
    void Start()
    {
        if (IsClient)
            StartCoroutine(SearchXROriginRootCoroutine());
        else if (IsServer)
            GetValidComponentsServerSide();
    }

    private void Update()
    {
        if(IsClient)
        {
            if (localXROriginRoot)
            {
                if (localXROriginRoot.playerId == lookingId.Value)
                {
                    SyncBofyTransform();

                    SyncHeadTransform();

                    SyncLeftHandTransform();
                    SyncLeftHandAnimator();

                    SyncRightHandTransform();
                    SyncRightHandAnimator();
                }
            }
        }
    }

    void SyncBofyTransform()
    {
        SyncBodyTransformServerRpc(localXROriginRoot.gameObject.transform.localPosition, localXROriginRoot.gameObject.transform.localRotation, localXROriginRoot.gameObject.transform.localScale);

        transform.localPosition = localXROriginRoot.gameObject.transform.localPosition;
        transform.localRotation = localXROriginRoot.gameObject.transform.localRotation;
        transform.localScale = localXROriginRoot.gameObject.transform.localScale;
    }

    [ServerRpc(RequireOwnership = false)]
    void SyncBodyTransformServerRpc(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        transform.localPosition = position;
        transform.localRotation = rotation;
        transform.localScale = scale;
    }

    void SyncHeadTransform()
    {
        if (HeadOffset && localXROriginRoot.HeadOrigin)
        {
            SyncHeadTransformServerRpc(localXROriginRoot.HeadOrigin.transform.localPosition, localXROriginRoot.HeadOrigin.transform.localRotation, localXROriginRoot.HeadOrigin.transform.localScale);

            HeadOffset.transform.localPosition = localXROriginRoot.HeadOrigin.transform.localPosition;
            HeadOffset.transform.localRotation = localXROriginRoot.HeadOrigin.transform.localRotation;
            HeadOffset.transform.localScale = localXROriginRoot.HeadOrigin.transform.localScale;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SyncHeadTransformServerRpc(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        if(HeadOffset)
        {
            HeadOffset.transform.localPosition = position;
            HeadOffset.transform.localRotation = rotation;
            HeadOffset.transform.localScale = scale;
        }
    }

    void SyncLeftHandTransform()
    {
        if (LeftHandOffset && localXROriginRoot.LeftHandOrigin)
        {
            SyncLeftHandTransformServerRpc(localXROriginRoot.LeftHandOrigin.transform.localPosition, localXROriginRoot.LeftHandOrigin.transform.localRotation, localXROriginRoot.LeftHandOrigin.transform.localScale);

            LeftHandOffset.transform.localPosition = localXROriginRoot.LeftHandOrigin.transform.localPosition;
            LeftHandOffset.transform.localRotation = localXROriginRoot.LeftHandOrigin.transform.localRotation;
            LeftHandOffset.transform.localScale = localXROriginRoot.LeftHandOrigin.transform.localScale;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SyncLeftHandTransformServerRpc(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        if(LeftHandOffset)
        {
            LeftHandOffset.transform.localPosition = position;
            LeftHandOffset.transform.localRotation = rotation;
            LeftHandOffset.transform.localScale = scale;
        }
    }

    void SyncLeftHandAnimator()
    {
        if(LeftHandAnimatorScript && LeftHandActionBasedController)
        {
            float triggerTarget = LeftHandActionBasedController.activateAction.action.ReadValue<float>();
            float gripTarget = LeftHandActionBasedController.activateAction.action.ReadValue<float>();

            SyncLeftHandAnimatorServerRpc(triggerTarget, gripTarget);

            LeftHandAnimatorScript.SetTrigger(triggerTarget);
            LeftHandAnimatorScript.SetGrip(gripTarget);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SyncLeftHandAnimatorServerRpc(float triggerTarget, float gripTarget)
    {
        if(LeftHandAnimatorScript)
        {
            LeftHandAnimatorScript.SetTrigger(triggerTarget);
            LeftHandAnimatorScript.SetGrip(gripTarget);
        }
    }

    void SyncRightHandTransform()
    {
        if (RightHandOffset && localXROriginRoot.RightHandOrigin)
        {
            SyncRightHandTransformServerRpc(localXROriginRoot.RightHandOrigin.transform.localPosition, localXROriginRoot.RightHandOrigin.transform.localRotation, localXROriginRoot.RightHandOrigin.transform.localScale);

            RightHandOffset.transform.localPosition = localXROriginRoot.RightHandOrigin.transform.localPosition;
            RightHandOffset.transform.localRotation = localXROriginRoot.RightHandOrigin.transform.localRotation;
            RightHandOffset.transform.localScale = localXROriginRoot.RightHandOrigin.transform.localScale;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SyncRightHandTransformServerRpc(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        if (RightHandOffset)
        {
            RightHandOffset.transform.localPosition = position;
            RightHandOffset.transform.localRotation = rotation;
            RightHandOffset.transform.localScale = scale;
        }
    }

    void SyncRightHandAnimator()
    {
        if (RightHandAnimatorScript && RightHandActionBasedController)
        {
            float triggerTarget = RightHandActionBasedController.activateAction.action.ReadValue<float>();
            float gripTarget = RightHandActionBasedController.activateAction.action.ReadValue<float>();

            SyncRightHandAnimatorServerRpc(triggerTarget, gripTarget);

            RightHandAnimatorScript.SetTrigger(triggerTarget);
            RightHandAnimatorScript.SetGrip(gripTarget);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SyncRightHandAnimatorServerRpc(float triggerTarget, float gripTarget)
    {
        if (RightHandAnimatorScript)
        {
            RightHandAnimatorScript.SetTrigger(triggerTarget);
            RightHandAnimatorScript.SetGrip(gripTarget);
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

                GetValidComponentsClientSide();

                break;
            }
        }

        yield return null;
    }

    void GetValidComponentsClientSide()
    {
        //////////////////////// BODY ////////////////////////
        GetComponent<TransformNetworkSync>().enabled = false;

        //////////////////////// HEAD ////////////////////////
        if (HeadOffset) HeadOffset.GetComponent<TransformNetworkSync>().enabled = false;

        //////////////////////// LEFT HAND ////////////////////////
        if (LeftHandOffset)
        {
            LeftHandOffset.GetComponent<TransformNetworkSync>().enabled = false;
        }

        if (LeftHandAnimator)
        {
            LeftHandAnimator.GetComponent<HandNetworkController>().enabled = false;
            LeftHandAnimatorScript = LeftHandAnimator.GetComponent<Hand>();
        }

        LeftHandActionBasedController = localXROriginRoot.LeftHandOrigin.GetComponent<ActionBasedController>();

        //////////////////////// RIGHT HAND ////////////////////////
        if (RightHandOffset)
        {
            RightHandOffset.GetComponent<TransformNetworkSync>().enabled = false;
        }

        if (RightHandAnimator)
        {
            RightHandAnimator.GetComponent<HandNetworkController>().enabled = false;
            RightHandAnimatorScript = RightHandAnimator.GetComponent<Hand>();
        }

        RightHandActionBasedController = localXROriginRoot.RightHandOrigin.GetComponent<ActionBasedController>();
    }

    void GetValidComponentsServerSide()
    {
        //////////////////////// LEFT HAND ////////////////////////
        if (LeftHandAnimator)
        {
            LeftHandAnimatorScript = LeftHandAnimator.GetComponent<Hand>();
        }

        //////////////////////// RIGHT HAND ////////////////////////
        if (RightHandAnimator)
        {
            RightHandAnimatorScript = RightHandAnimator.GetComponent<Hand>();
        }
    }
}
