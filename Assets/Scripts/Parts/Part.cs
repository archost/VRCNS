using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.XR.Interaction.Toolkit;
using VREventArgs;

[RequireComponent(typeof(Rigidbody), typeof(PartAttacher), typeof(AudioController))]
public class Part : MonoBehaviour, ITargetable
{
    public static int floorCollideCounter = 0;

    private Outline outline;
    private Rigidbody rb;
    private AudioController audioCon;
    private Collider col;
    private PartAnimationController animationController;
    private XRSimpleInteractable sInteractable;

    private XRGrabInteractable grabInteractable;

    private PartAttacher partAttacher;

    private Transform playerTransform;

    private bool isTarget = false;

    private bool isHolding = false;

    private bool isSelected = false;

    private bool isAssembly = false;

    public bool IsHolding => isHolding;

    public int PartID { get; private set; }

    public bool IgnoreErrors => ignoreErrors;

    public bool IsFixed => state == PartState.Fixed;

    [SerializeField]
    private PartState state;

    [SerializeField]
    private PartData partData;

    [SerializeField]
    private bool ignoreErrors;

    public void Attach()
    {
        UpdateState(PartState.Fixed);
        foreach (var item in GetComponentsInChildren<Collider>())
        {
            item.isTrigger = true;
        }
        if (animationController != null && isAssembly) animationController.PlayAnimation(PartAnimationController.PartAnimationType.Assembly);
        else Install();
    }

    [ContextMenu("Detach request")]
    public void DetachRequest() => DetachRequest(new SelectEnterEventArgs());
    
    public void DetachRequest(SelectEnterEventArgs e)
    {
        sInteractable.enabled = false;
        UpdateState(PartState.Fixed);
        if (animationController != null) animationController.PlayAnimation(PartAnimationController.PartAnimationType.Disassembly);
        else Detach();
    }

    private void Detach()
    {
        UpdateState(PartState.Await);
    }

    public void Install(bool silent = false)
    {
        UpdateState(PartState.Installed);
        if (silent) return;
        outline.enabled = false;
        isTarget = false;
        audioCon.TryPlayClip("installed");
        StageController.OnPartInstalled.Invoke(new (this));
    }

    public void AnimationFinished()
    {     
        animationController.DisableAnimator();
        if (isAssembly)
            Install();
        else
            Detach();
    }

    [ContextMenu("Test1")]
    public void Test1()
    {
        Selected(true);
    }

    [ContextMenu("Test2")]
    public void Test2()
    {
        Selected(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            floorCollideCounter++;
            audioCon.PlayClip("fall");
            if (collision.relativeVelocity.y < 2f)
                Debug.Log("This fall doesn't count", this.gameObject);
            else
            {
                StageController.OnMadeMistake.Invoke(new (this));
            }
        }
    }

    private void OnEnable()
    {
        if (partData == null)
        {
            Debug.LogError("Part has null PartData!", this.gameObject);
            PartID = 0;

        }
        else
        {
            PartID = partData.ID;

        }

        audioCon = GetComponent<AudioController>();
        animationController = GetComponent<PartAnimationController>();
        animationController.Init(partData);
        grabInteractable = GetComponent<XRGrabInteractable>();
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        outline = gameObject.AddComponent<Outline>();
        partAttacher = GetComponent<PartAttacher>();

        outline.enabled = false;

        UpdateState(state);

        if (grabInteractable != null)
        {
            grabInteractable.firstSelectEntered.AddListener(OnSelectEnter);
            grabInteractable.lastSelectExited.AddListener(OnSelectExit);
        }
    }

    private void Start()
    {
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = ProjectPreferences.instance.highlightOutlineColor;
        outline.OutlineWidth = ProjectPreferences.instance.outlineWidth;

        playerTransform = Camera.main.gameObject.transform;


    }

    private void Update()
    {
        if (isHolding)
        {
            col.isTrigger = Vector3.Distance(playerTransform.position + Vector3.down * 0.7f, transform.position) < 1.2f;
        }
    }

    public void SetAsTarget(TargetSetEventArgs e)
    {
        var args = e as PartSetAsTargetEventArgs;
        isTarget = true;
        isAssembly = args.AssemblyType == GameAssemblyType.Assembly;
        if (!isAssembly)
        {
            sInteractable = gameObject.GetComponent<XRSimpleInteractable>();
            sInteractable.enabled = true;
            sInteractable.selectEntered.AddListener(DetachRequest);
        }
        else
        {
            if (state != PartState.Idle)
                UpdateState(PartState.Idle);
        }
        if (ProjectPreferences.instance.gameMode == GameMode.Training)
            outline.enabled = true;
    }

    private void OnSelectEnter(SelectEnterEventArgs args) => Selected(true);

    private void OnSelectExit(SelectExitEventArgs args) => Selected(false);

    public void WrongPartDisplay()
    {
        outline.enabled = true;
        outline.OutlineColor = ProjectPreferences.instance.wrongOutlineColor;
        Invoke(nameof(ResetDisplay), 1.5f);
    }

    private void ResetDisplay()
    {
        outline.OutlineColor = ProjectPreferences.instance.highlightOutlineColor;
        outline.enabled = false;
    }

    private void ResetCollider()
    {
        if (!isSelected) return;
        isHolding = true;
    }

    private void Selected(bool isSelected)
    {
        this.isSelected = isSelected;
        if (isSelected)
        {
            col.isTrigger = true;
            Invoke(nameof(ResetCollider), 0.3f);
        }
        else
        {
            isHolding = false;
            col.isTrigger = false;
        }
        if (isTarget && ProjectPreferences.instance.IsTraining)
        {
            outline.enabled = !isSelected;
        }
        StageController.OnPartSelected.Invoke(new(this, transform, partData, isSelected));
    }

    private void UpdateState(PartState newState)
    {
        state = newState;
        switch (newState)
        {
            case PartState.Idle:
                rb.isKinematic = false;
                col.isTrigger = false;
                if (grabInteractable != null) grabInteractable.enabled = true;
                break;
            case PartState.Fixed:
                rb.isKinematic = true;
                col.isTrigger = true;
                if (grabInteractable != null) grabInteractable.enabled = false;
                break;
            case PartState.Installed:
                rb.isKinematic = true;
                col.isTrigger = false;
                if (grabInteractable != null) grabInteractable.enabled = false;
                break;
            case PartState.Await:
                rb.isKinematic = true;
                col.isTrigger = false;
                if (grabInteractable != null) grabInteractable.enabled = true;
                break;
            default:
                Debug.LogError("Wrong newState type", this.gameObject);
                return;
        }
    }
}

public enum PartState
{
    Idle,
    Fixed,
    Installed,
    Await
}
