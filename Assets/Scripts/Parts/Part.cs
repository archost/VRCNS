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
    private AudioSource audioSource;
    //private Collider col;
    private PartAnimationController animationController;

    public XRGrabInteractable GrabInteractable { get; private set; }

    private PartPresenter partPresenter;
    private PartAttacher partAttacher;

    private bool isTarget = false;

    public int PartID { get; private set; }  

    [SerializeField]
    private PartState state;

    [SerializeField]
    private PartData partData;

    public void Attach()
    {        
        UpdateState(PartState.Fixed);
        if (animationController != null) animationController.ToogleAnimator();
        else Install();
    }

    public void Install()
    {
        UpdateState(PartState.Installed);
        if (audioSource != null) audioSource.Play();
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
        audioSource = GetComponent<AudioSource>();
        animationController = GetComponent<PartAnimationController>();
        GrabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        outline = gameObject.AddComponent<Outline>();
        partAttacher = GetComponent<PartAttacher>();

        UpdateState(state);

        GrabInteractable.firstSelectEntered.AddListener(OnSelectEnter);
        GrabInteractable.lastSelectExited.AddListener(OnSelectExit);
    }

    private void Start()
    {        
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = ProjectPreferences.instance.outlineColor;
        outline.OutlineWidth = ProjectPreferences.instance.outlineWidth;

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

    private void ProcessSetTarget()
    {
        isTarget = true;
        outline.enabled = true;
    }

    private void OnSelectEnter(SelectEnterEventArgs args) => OnSelectEvent(true);

    private void OnSelectExit(SelectExitEventArgs args) => OnSelectEvent(false);

    private void OnSelectEvent(bool isSelected)
    {
        if (isTarget)
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
