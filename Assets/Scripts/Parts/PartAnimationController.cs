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

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        enabled = false;
    }

    public void ToogleAnimator()
    {
        enabled = true;
        animator.enabled = true;
    }

    private void Update()
    {
        if (!animator.isActiveAndEnabled) return;

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
