using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class ObjectScale : MonoBehaviour
{
    private Vector3 scaleFactor = new Vector3(0.5f,0.5f,0.5f);

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
        if (PlayerState.Singleton.playerState == PlayerType.GAMEMASTER)
        {

        }

        if (PlayerState.Singleton.playerState == PlayerType.PLAYER)
        {

        }

        if (PlayerState.Singleton.playerState == PlayerType.EDIT)
        {
            transform.localScale -= scaleFactor;
        }
    }

    void ObjectIsNotGrabbed(SelectExitEventArgs args)
    {
        if (PlayerState.Singleton.playerState == PlayerType.GAMEMASTER)
        {

        }

        if (PlayerState.Singleton.playerState == PlayerType.PLAYER)
        {

        }

        if (PlayerState.Singleton.playerState == PlayerType.EDIT)
        {
            transform.localScale += scaleFactor;
        }
    }
}