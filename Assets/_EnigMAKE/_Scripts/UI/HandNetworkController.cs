using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Hand))]
public class HandNetworkController : NetworkBehaviour
{
    NetworkVariable<float> TriggerTarget = new NetworkVariable<float>();
    NetworkVariable<float> GripTarget = new NetworkVariable<float>();

    Hand handScript;

    private void Start()
    {
        handScript = GetComponent<Hand>();
    }

    private void Update()
    {
        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        if (IsOwner)
        {
            if (IsServer)
            {
                TriggerTarget.Value = handScript.GetTrigger();
                GripTarget.Value = handScript.GetGrip();
            }
            else
            {
                AnimatorSyncServerRpc(handScript.GetTrigger(), handScript.GetGrip());
            }
        }
        else
        {
            handScript.SetTrigger(TriggerTarget.Value);
            handScript.SetGrip(GripTarget.Value);
        }
    }

    [ServerRpc]
    void AnimatorSyncServerRpc(float triggerTarget, float gripTarget)
    {
        TriggerTarget.Value = triggerTarget;
        GripTarget.Value = gripTarget;
    }
}
