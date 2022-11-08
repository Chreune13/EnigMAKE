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
    //private float thumbTarget;

    private float triggerCurrent;
    private float gripCurrent;
    //private float thumbCurrent;

    public float speed;

    private string animatorTriggerParam = "Trigger";
    private string animatorGripParam = "Grip";
    //private string animatorThumbParam = "Thumb";

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

    internal void SetGrip(float v)
    {
        gripTarget = v;
    }

    //internal void SetThumb(Vector2 v2)
    //{
    //    thumbTarget = v2.y;
    //}

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

        //if (thumbCurrent != thumbTarget)
        //{
        //    thumbCurrent = Mathf.MoveTowards(thumbCurrent, thumbTarget, Time.deltaTime * speed);
        //    animator.SetFloat(animatorThumbParam, thumbCurrent);
        //}
    }
}
