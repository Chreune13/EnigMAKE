using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour
{
    Animator animator;
    private float triggerTarget;
    private float gripTarget;

    private float triggerCurrent;
    private float gripCurrent;

    public float speed;

    private string animatorTriggerParam = "Trigger";
    private string animatorGripParam = "Grip";

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimateHand();
    }

    internal void SetTrigger(float v)
    {
        triggerTarget = v;
    }

    public float GetTrigger()
    {
        return triggerTarget;
    }

    internal void SetGrip(float v)
    {
        gripTarget = v;
    }

    public float GetGrip()
    {
        return gripTarget;
    }

    void AnimateHand()
    {
        if (triggerCurrent != triggerTarget)
        {
            triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * speed);
            animator.SetFloat(animatorTriggerParam, triggerCurrent);
        }

        if (gripCurrent != gripTarget)
        {
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * speed);
            animator.SetFloat(animatorGripParam, gripCurrent);
        }
    }
}
