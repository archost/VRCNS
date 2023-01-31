using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class JointPoint : MonoBehaviour
{
    public static GameObject ArrowPrefab = null;

    private bool IsVR;

    //!
    public PartData suitablePart;

    [SerializeField]
    private Vector3 fixedPosition = Vector3.zero;

    [SerializeField]
    private Quaternion fixedRotation = Quaternion.identity;

    private GameObject currArrow = null;

    private float arrowOffset = 0.5f;

    public UnityAction<Part, Vector3, Quaternion, bool> OnPartAttached;

    private bool IsNotHolding => !HandRayController.instance.IsLeftHolding && !HandRayController.instance.IsRightHolding;

    private void Awake()
    {
        if (ArrowPrefab == null) ArrowPrefab = Resources.Load<GameObject>("Prefabs/Arrow");
    }

    private void Start()
    {
        IsVR = ProjectPreferences.instance.VRTesting;
        if (ArrowPrefab == null) Debug.LogError("No arrow prefab found!");
        Collider col = GetComponent<Collider>();
        if (col.isTrigger == false) Debug.LogError("Collider is not trigger!", gameObject);
        arrowOffset = col.bounds.size.MaxComponent() / 2;
        if (suitablePart == null) Debug.LogError("No suitable PartData found!", gameObject);
        ArrowSetup();
    }

    private void ArrowSetup()
    {
        if (ArrowPrefab == null) return;
        if (currArrow != null) Destroy(currArrow);
        currArrow = Instantiate(ArrowPrefab);
        Vector3 dv = fixedPosition - transform.localPosition;
        Vector3 dist = (-dv).normalized * arrowOffset;
        currArrow.transform.position = transform.position + dist;
        currArrow.transform.LookAt(transform.position);
    }

    public void ForceAttach(Part p, bool toFix = true)
    {
        OnPartAttached?.Invoke(p, fixedPosition, fixedRotation, toFix);
        gameObject.SetActive(false);
        if (currArrow != null) Destroy(currArrow);
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
