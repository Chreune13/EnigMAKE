using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class ObjectManager : MonoBehaviour
{
    //------------------------ Object raycast ----------------------


    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (!isGrabbed)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 100.0f))
            {
                //print("Found an object - distance: " + hit.distance);
                //print("Found an object - normal: " + hit.normal);
                Debug.DrawRay(ray.origin, ray.direction, Color.blue);

                transform.localRotation = hit.transform.localRotation;
            }
        }
    }

    //--------------------------------------------------------------

    //-------------------- Object Scale on Grab --------------------

    private Vector3 doorScaleFactor = new Vector3(0.8f,0.8f,0.8f);
    private Vector3 keyScaleFactor = new Vector3(0.5f, 0.5f, 0.5f);

    XRGrabInteractable grabInteractable;
    private bool isGrabbed = false;

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
        isGrabbed = true;

        if (PlayerState.Singleton.playerState == PlayerType.GAMEMASTER)
        {
            //
        }

        if (PlayerState.Singleton.playerState == PlayerType.PLAYER)
        {
            //
        }

        if (PlayerState.Singleton.playerState == PlayerType.EDIT)
        {
            if(gameObject.tag == "Door")
                transform.localScale -= doorScaleFactor;
            if(gameObject.tag == "Key")
                transform.localScale += keyScaleFactor;
        }
    }

    public void ObjectIsNotGrabbed(SelectExitEventArgs args)
    {
        isGrabbed = false;

        if (PlayerState.Singleton.playerState == PlayerType.GAMEMASTER)
        {
            //
        }

        if (PlayerState.Singleton.playerState == PlayerType.PLAYER)
        {
            //
        }

        if (PlayerState.Singleton.playerState == PlayerType.EDIT)
        {
            if (gameObject.tag == "Door")
                transform.localScale += doorScaleFactor;
            if (gameObject.tag == "Key")
                transform.localScale -= keyScaleFactor;
        }
    }

    //-------------------------------------------------------------
}