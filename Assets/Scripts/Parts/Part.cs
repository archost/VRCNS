using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody), typeof(PartAttacher), typeof(AudioController))]
public class Part : MonoBehaviour
{
    public static int floorCollideCounter = 0;

    private Outline outline;
    private Rigidbody rb;
    private AudioController audioCon;
    private Collider col;
    private PartAnimationController animationController;
    private XRSimpleInteractable sInteractable;

    public XRGrabInteractable GrabInteractable { get; private set; }

    private PartPresenter partPresenter;
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
        /*
        if (!isAssembly && !isTarget)
        {
            UpdateState(PartState.Installed);
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                if (!child.TryGetComponent<JointPoint>(out _))
                {
                    child.SetActive(true);
                }
            }
            return;
        }
        */
        UpdateState(PartState.Fixed);
        col.isTrigger = true;
        foreach (var item in GetComponentsInChildren<Collider>())
        {
            item.isTrigger = true;
        }
        if (animationController != null) animationController.ToogleAnimator();
        else Install();
    }

    public void Detach(SelectEnterEventArgs e)
    {
        sInteractable.enabled = false;
        // animation
        UpdateState(PartState.Idle);
        GrabInteractable.enabled = true;
    }

    public void DisassemblyInstall()
    {
        if (isAssembly) return;
        UpdateState(PartState.Installed);
        col.isTrigger = false;
        foreach (var item in GetComponentsInChildren<Collider>())
        {
            item.isTrigger = false;
        }
        if (GrabInteractable != null) GrabInteractable.enabled = false;
    }

    public void Install()
    {
        UpdateState(PartState.Installed);
        col.isTrigger = false;
        foreach (var item in GetComponentsInChildren<Collider>())
        {
            item.isTrigger = false;
        }
        audioCon.TryPlayClip("installed");
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            floorCollideCounter++;
            audioCon.PlayClip("fall");
            if (collision.relativeVelocity.y < 2f)
                Debug.Log("This fall doesn't count");
            else
                partPresenter.Send(new CommandProcessMistake(this.partPresenter), null);
            //Debug.Log($"Current floor collides: {floorCollideCounter}");
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


    }

    private void Update()
    {
        if (isHolding)
        {
            col.isTrigger = Vector3.Distance(playerTransform.position + Vector3.down * 0.7f, transform.position) < 1.2f;
        }
    }

    private void ProcessSetTarget(GameAssemblyType assemblyType, PartState? newState)
    {
        if (newState != null) UpdateState(newState.Value);
        isTarget = true;
        isAssembly = assemblyType == GameAssemblyType.Assembly;
        if (!isAssembly)
        {
            sInteractable = gameObject.GetComponent<XRSimpleInteractable>();
            sInteractable.enabled = true;
            sInteractable.selectEntered.AddListener(Detach);
        }
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
        if (!isSelected) return;
        isHolding = true;
        //col.isTrigger = false;
    }

    private void OnSelectEvent(bool isSelected)
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
