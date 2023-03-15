using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody), typeof(PartAttacher))]
public class Part : MonoBehaviour
{
    private Outline outline;
    private Rigidbody rb;
    private AudioController audioCon;
    private Collider col;
    private PartAnimationController animationController;

    public XRGrabInteractable GrabInteractable { get; private set; }

    private PartPresenter partPresenter;
    private PartAttacher partAttacher;

    private Transform playerTransform;

    private bool isTarget = false;

    private bool isHolding = false;

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
        col.isTrigger = true;
        foreach (var item in GetComponentsInChildren<Collider>())
        {
            item.isTrigger = true;
        }
        if (animationController != null) animationController.ToogleAnimator();
        else Install();
    }

    public void Install()
    {
        UpdateState(PartState.Installed);
        col.isTrigger = false;
        foreach (var item in GetComponentsInChildren<Collider>())
        {
            item.isTrigger = false;
        }
        if (audioCon != null) audioCon.PlayClip("installed");
        partPresenter.Send(new CommandFinished(this.partPresenter), null);
    }

    public void AnimationFinished()
    {
        isTarget = false;
        outline.enabled = false;
        Install();
    }

    [ContextMenu("Test1")]
    public void Test1()
    {
        OnSelectEvent(true);
    }

    [ContextMenu("Test2")]
    public void Test2()
    {
        OnSelectEvent(false);
    }

    public PartPresenter InitPartPresenter(Mediator mediator)
    {
        partPresenter = new PartPresenter(mediator, partData);
        partPresenter.OnJointPointToogle += partAttacher.ToogleJointPoint;
        partPresenter.OnSetTarget += ProcessSetTarget;
        return partPresenter;
    }

    private void Awake()
    {
        audioCon = GetComponent<AudioController>();
        animationController = GetComponent<PartAnimationController>();
        GrabInteractable = GetComponent<XRGrabInteractable>();
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        outline = gameObject.AddComponent<Outline>();
        partAttacher = GetComponent<PartAttacher>();

        UpdateState(state);

        if (GrabInteractable != null)
        {
            GrabInteractable.firstSelectEntered.AddListener(OnSelectEnter);
            GrabInteractable.lastSelectExited.AddListener(OnSelectExit);
        }        
    }

    private void Start()
    {        
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = ProjectPreferences.instance.highlightOutlineColor;
        outline.OutlineWidth = ProjectPreferences.instance.outlineWidth;

        playerTransform = Camera.main.gameObject.transform;

        if (partData == null)
        {
            Debug.LogError("Part has null PartData!", this.gameObject);
            PartID = 0;
            
        }
        else
        {
            PartID = partData.ID;

        }
    }

    private void Update()
    {
        if (isHolding)
        {
            
            col.isTrigger = Vector3.Distance(playerTransform.position + Vector3.down * 0.7f, transform.position) < 1.2f;
        }
    }

    private void ProcessSetTarget(PartState? newState)
    {
        if (newState != null) UpdateState(newState.Value);
        isTarget = true;
        if (ProjectPreferences.instance.gameMode == GameMode.Training)
            outline.enabled = true;
    }

    private void OnSelectEnter(SelectEnterEventArgs args) => OnSelectEvent(true);

    private void OnSelectExit(SelectExitEventArgs args) => OnSelectEvent(false);

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
        isHolding = true;
        //col.isTrigger = false;
    }

    private void OnSelectEvent(bool isSelected)
    {
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
        partPresenter.Send(new CommandHelperUpdate(partPresenter, transform, partData, isSelected), null);
    }

    private void UpdateState(PartState newState)
    {
        state = newState;
        switch (newState)
        {
            case PartState.Idle:
                rb.isKinematic = false;
                outline.enabled = false;
                break;
            case PartState.Holding:
                rb.isKinematic = false;
                outline.enabled = false;
                break;
            case PartState.Fixed:
                rb.isKinematic = true;
                outline.enabled = false;
                break;
            case PartState.Installed:
                rb.isKinematic = true;
                outline.enabled = false;
                break;
            default:
                Debug.LogError("Wrong newState type");
                return;
        }
    }
}

public enum PartState
{
    Idle,
    Holding,
    Fixed,
    Installed
}
