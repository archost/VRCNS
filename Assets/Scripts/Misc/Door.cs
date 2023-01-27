using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    [ContextMenu("Test")]
    public void Open()
    {
        animator.SetBool("IsOpen", true);
    }
}
