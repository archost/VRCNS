using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.XR.CoreUtils;

[RequireComponent(typeof(Animator))]
public class PartAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    public void ToogleAnimator()
    {
        animator.enabled = true;
    }

    // public InputActionProperty rightActivate;
    // public InputActionProperty leftActivate;

    private void Update()
    {
        if (!animator.isActiveAndEnabled) return;

        /*
        bool isRightHovering = HandRayController.instance.IsRightHovering;
        bool isLeftHovering = HandRayController.instance.IsLeftHovering;
        
            if ((isRightHovering && rightActivate.action.ReadValue<float>() > 0.1f) || (isLeftHovering && leftActivate.action.ReadValue<float>() > 0.1f))
            {
                animator.speed = 1;
            }
            else
            {
                animator.speed = 0;
            }
        */

        if (animator.GetBool("Play") == true)
        {
            //костыль 10/10
            animator.speed = 1;
            
        }
        else
        {
            animator.speed = 0;
        }
    }
}
