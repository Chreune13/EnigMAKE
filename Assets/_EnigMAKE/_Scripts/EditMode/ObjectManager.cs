using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class ObjectManager : MonoBehaviour
{
    new Collider collider;
    new Renderer renderer;

    public void Start()
    {
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
    }


    //------------------------ Object raycast ----------------------


    private void FixedUpdate()
    {   
        if ((int)PlayerState.Singleton.playerState == 2)
        {
            if(!isGrabbed) stickToSurface();
        }
    }

    private void stickToSurface()
    {
        //Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        Transform tf=transform;
        Debug.Log("a"+tf.rotation);

        if(renderer == null && collider == null)
        {
            Transform parentTransform = GetComponentInParent<Transform>();

            //if (Physics.Raycast(parentTransform.position /*- new Vector3(0, collider.bounds.size.y / 2, 0)*/, Vector3.down, out hit, 0.3f))
            //{
            //    //print("Found an object - distance: " + hit.distance);
            //    //print("Found an object - normal: " + hit.normal);
            //    //Debug.DrawRay(ray.origin, ray.direction, Color.blue);

            //    // Change orientation of the objet
            //    parentTransform.up = hit.transform.up;

            //    //Change the position of the object so it "sticks" to a surface 
            //    float YPos = hit.transform.position.y /*+ collider.bounds.size.y / 2*/ + hit.collider.bounds.size.y;
            //    parentTransform.position = new Vector3(parentTransform.position.x, YPos, parentTransform.position.z);
            //}
        }

        if(renderer != null && collider == null)
        {
            if (Physics.Raycast(transform.position - new Vector3(0, renderer.bounds.size.y / 2, 0), Vector3.down, out hit, 0.3f))
            {
                //print("Found an object - distance: " + hit.distance);
                //print("Found an object - normal: " + hit.normal);
                //Debug.DrawRay(ray.origin, ray.direction, Color.blue);

                // Change orientation of the objet
                transform.up = hit.transform.up;

                //Change the position of the object so it "sticks" to a surface 
                float YPos = hit.transform.position.y + renderer.bounds.size.y / 2 + hit.collider.bounds.size.y;
                transform.position = new Vector3(transform.position.x, YPos, transform.position.z);
                transform.rotation = tf.rotation;

            }
        }

        if(renderer == null && collider != null)
        {
            if (Physics.Raycast(transform.position - new Vector3(0, collider.bounds.size.y / 2, 0), Vector3.down, out hit, 0.3f))
            {
                //print("Found an object - distance: " + hit.distance);
                //print("Found an object - normal: " + hit.normal);
                //Debug.DrawRay(ray.origin, ray.direction, Color.blue);

                // Change orientation of the objet
                transform.up = hit.transform.up;

                //Change the position of the object so it "sticks" to a surface 
                float YPos = hit.transform.position.y + collider.bounds.size.y / 2 + hit.collider.bounds.size.y;
                transform.position = new Vector3(transform.position.x, YPos, transform.position.z);
                transform.rotation = tf.rotation;

            }
        }

        if(renderer != null && collider != null)
        {
            if (Physics.Raycast(transform.position /*- new Vector3(0, collider.bounds.size.y / 2, 0)*/, Vector3.down, out hit, 0.3f))
            {
                //print("Found an object - distance: " + hit.distance);
                //print("Found an object - normal: " + hit.normal);
                //Debug.DrawRay(ray.origin, ray.direction, Color.blue);

                // Change orientation of the objet
                transform.up = hit.transform.up;

                //Change the position of the object so it "sticks" to a surface 
                float YPos = hit.transform.position.y /*+ collider.bounds.size.y / 2*/ + hit.collider.bounds.size.y;
                transform.position = new Vector3(transform.position.x, YPos, transform.position.z);
                transform.rotation = tf.rotation;

            }
        }
        Debug.Log("b" + transform.rotation);
    }

    /*private Vector3 checkTransformRotation()
    {
        if(transform.localRotation.eulerAngles.x <= 50 ||
            transform.localRotation.eulerAngles.x >= -50 &

            transform.localRotation.eulerAngles.z <= 50 ||
            transform.localRotation.eulerAngles.z >= -50)
        {
            Debug.Log("Down");
            return Vector3.down;
        }

        if (transform.localRotation.eulerAngles.x <= -50 ||
            transform.localRotation.eulerAngles.x >= -100 &

            transform.localRotation.eulerAngles.z <= -50 ||
            transform.localRotation.eulerAngles.z >= -100)
        {
            Debug.Log("Left");
            return Vector3.left;
        }

        if (transform.localRotation.eulerAngles.x <= 50 ||
            transform.localRotation.eulerAngles.x >= 100 &

            transform.localRotation.eulerAngles.z <= 50 ||
            transform.localRotation.eulerAngles.z >= 100)
        {
            Debug.Log("Right");
            return Vector3.right;
        }

        if (transform.localRotation.eulerAngles.x <= -100 ||
            transform.localRotation.eulerAngles.x >= 100 &

            transform.localRotation.eulerAngles.z <= -100 ||
            transform.localRotation.eulerAngles.z >= 100)
        {
            Debug.Log("Up");
            return Vector3.up;
        }

        Debug.Log("Default");
        return Vector3.down;
    }*/

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
            if(gameObject.tag == "Key" || gameObject.tag == "Locke")
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
            if (gameObject.tag == "Key" || gameObject.tag == "Locke")
                transform.localScale -= keyScaleFactor;
        }
    }

    //-------------------------------------------------------------
}