using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class ObjectScale : MonoBehaviour
{
    private Vector3 doorScaleFactor = new Vector3(0.8f,0.8f,0.8f);
    private Vector3 keyScaleFactor = new Vector3(0.5f, 0.5f, 0.5f);

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
            if(gameObject.tag == "Door")
                transform.localScale -= doorScaleFactor;
            if(gameObject.tag == "Key")
                transform.localScale += keyScaleFactor;
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
            if (gameObject.tag == "Door")
                transform.localScale += doorScaleFactor;
            if (gameObject.tag == "Key")
                transform.localScale -= keyScaleFactor;
        }
    }
}