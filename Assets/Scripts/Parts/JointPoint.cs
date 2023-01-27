using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class JointPoint : MonoBehaviour
{
    private bool IsVR;

    //!
    public PartData suitablePart;

    [SerializeField]
    private Vector3 fixedPosition = Vector3.zero;

    [SerializeField]
    private Quaternion fixedRotation = Quaternion.identity;

    public UnityAction<Part, Vector3, Quaternion, bool> OnPartAttached;

    private bool IsNotHolding => !HandRayController.instance.IsLeftHolding && !HandRayController.instance.IsRightHolding;

    private void Start()
    {
        IsVR = ProjectPreferences.instance.VRTesting;
        if (GetComponent<Collider>().isTrigger == false) Debug.LogError("Collider is not trigger!", gameObject);
        if (suitablePart == null) Debug.LogError("No suitable PartData found!", gameObject);
    }

    public void ForceAttach(Part p, bool toFix = true)
    {
        OnPartAttached?.Invoke(p, fixedPosition, fixedRotation, toFix);
        gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Part p))
        {
            if (p.PartID == suitablePart.ID)
            {
                if (!IsVR || (IsNotHolding && IsVR))
                {
                    ForceAttach(p);
                }
            }
        }
    }
}
