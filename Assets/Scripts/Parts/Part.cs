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

    [ContextMenu("Test")]
    public void Test()
    {
        partAttacher.ToogleJointPoint(0);
    }

    public PartPresenter InitPartPresenter(Mediator mediator)
    {
        partPresenter = new PartPresenter(mediator, partData);
        partPresenter.OnJointPointToogle += partAttacher.ToogleJointPoint;
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
        
        UpdateState(state);

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
                outline.enabled = true;
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
