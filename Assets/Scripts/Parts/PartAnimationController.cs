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

    private AnimationClip asmClip;

    private AnimationClip disasmClip;

    public void Init(PartData data)
    {
        animator = GetComponent<Animator>();
        asmClip = data.assemblyClip;
        disasmClip = data.disassemblyClip;
        animator.enabled = false;
    }

    public void PlayAnimation(PartAnimationType animType)
    {
        if (!animator.isActiveAndEnabled) EnableAnimator();
        switch (animType)
        {
            case PartAnimationType.None:
                return;
            case PartAnimationType.Assembly:
                if (asmClip == null)
                {
                    Debug.LogError("No Assembly clip on this Part!", gameObject);
                    break;
                }
                animator.Play(asmClip.name);
                break;
            case PartAnimationType.Disassembly:
                if (disasmClip == null)
                {
                    Debug.LogError("No Disassembly clip on this Part!", gameObject);
                    break;
                }
                animator.Play(disasmClip.name);
                break;
            default:
                break;
        }
    }

    public void EnableAnimator()
    {
        animator.enabled = true;
    }

    public void DisableAnimator()
    {
        animator.enabled = false;
    }

    public enum PartAnimationType
    {
        None,
        Assembly,
        Disassembly
    }
}
