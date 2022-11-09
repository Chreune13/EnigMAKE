using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XROriginRoot : MonoBehaviour
{
    public static XROriginRoot Singleton;

    XROriginNetworkSync networkClone;

    [SerializeField]
    GameObject BodyOrigin;

    [SerializeField]
    GameObject HeadOrigin;

    [SerializeField]
    GameObject LeftHandOrigin;

    ActionBasedController LeftController;

    [SerializeField]
    GameObject RightHandOrigin;

    ActionBasedController RightController;

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Debug.LogError("Multiple instance of Singleton XROriginRoot!");
    }

    private void Start()
    {
        if (LeftHandOrigin)
            LeftController = LeftHandOrigin.GetComponent<ActionBasedController>();

        if (RightHandOrigin)
            RightController = RightHandOrigin.GetComponent<ActionBasedController>();
    }

    private void Update()
    {
        UpdateNetworkClone();
    }

    void UpdateNetworkClone()
    {
        if (networkClone)
        {
            if (networkClone.gameObject && BodyOrigin)
                SetTransformOfClone(networkClone.gameObject, BodyOrigin);

            if (networkClone.HeadOffset && HeadOrigin)
                SetTransformOfClone(networkClone.HeadOffset, HeadOrigin);

            if (networkClone.LeftHandOffset && LeftHandOrigin)
                SetTransformOfClone(networkClone.LeftHandOffset, LeftHandOrigin);

            if (networkClone.RightHandOffset && RightHandOrigin)
                SetTransformOfClone(networkClone.RightHandOffset, RightHandOrigin);

            if (networkClone.LeftHandAnimatorScript && LeftController)
                SetAnimatorOfClone(networkClone.LeftHandAnimatorScript, LeftController);

            if (networkClone.RightHandAnimatorScript && RightController)
                SetAnimatorOfClone(networkClone.RightHandAnimatorScript, RightController);
        }
    }

    void SetTransformOfClone(GameObject destination, GameObject source)
    {
        destination.transform.localPosition = source.transform.localPosition;
        destination.transform.localRotation = source.transform.localRotation;
        destination.transform.localScale = source.transform.localScale;
    }

    void SetAnimatorOfClone(Hand animatorScript, ActionBasedController controller)
    {
        animatorScript.SetTrigger(controller.activateAction.action.ReadValue<float>());
        animatorScript.SetGrip(controller.activateAction.action.ReadValue<float>());
    }

    public void SetNetworkClone(XROriginNetworkSync p_networkClone)
    {
        networkClone = p_networkClone;
    }
}
