using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class ObjectManager : MonoBehaviour
{
    [SerializeField]
    private float stickDistance = 0.3f;

    [SerializeField]
    private float rotationLerpSpeed = 5f;

    [SerializeField]
    private float gridSize = 0.5f;

    private Quaternion initialRotation;

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

    private void FixedUpdate()
    {
        initialRotation = transform.rotation;
        stickToSurface();
    }

    private void stickToSurface()
    {
        RaycastHit hit;
        if (!isGrabbed)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out hit, stickDistance))
            {
                // Keep the last rotation
                Vector3 target = new Vector3(0, initialRotation.eulerAngles.y, 0);
                transform.rotation = Quaternion.Euler(target);

                // Change the position of the object so it "sticks" to a surface 
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                transform.position = new Vector3(Mathf.Round(transform.position.x / gridSize) * gridSize,
                                                transform.position.y,
                                                Mathf.Round(transform.position.z / gridSize) * gridSize);
            }
        }
    }

    //-------------------- Object Scale on Grab --------------------

    private Vector3 doorScaleFactor = new Vector3(0.8f, 0.8f, 0.8f);
    private Vector3 keyScaleFactor = new Vector3(0.5f, 0.5f, 0.5f);

    public void ObjectIsGrabbed(SelectEnterEventArgs args)
    {
        isGrabbed = true;

        if (PlayerState.Singleton.playerState == PlayerType.EDIT)
        {
            if (gameObject.tag == "Door")
                transform.localScale -= doorScaleFactor;
            if (gameObject.tag == "Key" || gameObject.tag == "Locke")
                transform.localScale += keyScaleFactor;
        }
    }

    public void ObjectIsNotGrabbed(SelectExitEventArgs args)
    {
        isGrabbed = false;

        if (PlayerState.Singleton.playerState == PlayerType.EDIT)
        {
            if (gameObject.tag == "Door")
                transform.localScale += doorScaleFactor;
            if (gameObject.tag == "Key" || gameObject.tag == "Locke")
                transform.localScale -= keyScaleFactor;
        }
    }

    //-------------------------------------------------------------
}