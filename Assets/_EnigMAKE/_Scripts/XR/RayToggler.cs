using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRRayInteractor))]
public class RayToggler : MonoBehaviour
{
    [SerializeField] private InputActionReference actionReference = null;

    private XRRayInteractor rayInteractor = null;
    private bool isEnabled = false;

    private void Awake()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
    }
    private void OnEnable()
    {
        actionReference.action.started += ToggleRay;
        actionReference.action.canceled += ToggleRay;
    }

    private void OnDisable()
    {
        actionReference.action.started -= ToggleRay;
        actionReference.action.canceled -= ToggleRay;
    }

    private void ToggleRay(InputAction.CallbackContext context)
    {
        isEnabled = context.control.IsPressed();
        Debug.Log("pressed");
    }
    private void LateUpdate()
    {
        ApplyStatus();
    }
    private void ApplyStatus()
    {
        if (rayInteractor.enabled != isEnabled)
            rayInteractor.enabled = isEnabled;
    }
}
