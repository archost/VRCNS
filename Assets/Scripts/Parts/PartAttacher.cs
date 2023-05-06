using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PartAttacher : MonoBehaviour
{
    public UnityAction<Part> OnPartAttached;

    [SerializeField]
    private List<JointPoint> jointPoints;

    [SerializeField]
    private bool isAssemblyAttacher;

    [ContextMenu("Test")]
    public void Test()
    {
        ToogleJointPoint(0);
    }

    public void ToogleJointPoint(int index)
    {
        if (index >= 0 && index < jointPoints.Count)
        {
            jointPoints[index].gameObject.SetActive(true);

        }
    }

    public bool TryToogleJointPointByPart(bool isAssembly, PartData data)
    {
        if (isAssembly ^ isAssemblyAttacher == true) return false;
        for (int i = 0; i < jointPoints.Count; i++)
        {
            if (jointPoints[i] == null || jointPoints[i].suitablePart == null)
            {
                Debug.LogError("WRONG PART", gameObject);
                return false;
            }
            if (jointPoints[i].suitablePart.ID == data.ID)
            {
                ToogleJointPoint(i);
                return true;
            }
        }
        return false;
    }

    public JointPoint GetJointPointByPartID(bool isAssembly, int partDataID)
    {
        if (isAssembly ^ isAssemblyAttacher == true) return null;
        for (int i = 0; i < jointPoints.Count; i++)
        {
            if (jointPoints[i] == null || jointPoints[i].suitablePart == null)
            {
                Debug.LogError("WRONG PART", gameObject);
                return null;
            }
            if (jointPoints[i].suitablePart.ID == partDataID)
            {
                return jointPoints[i];
            }
        }
        return null;
    }

    public void AttachPart(Part part, Vector3 offset, Quaternion rotation, bool toBeFixed)
    {
        if (toBeFixed)
        {
            //Debug.Log($"Part attaching {part.PartID}");
            if (part.GrabInteractable != null) part.GrabInteractable.enabled = false;
            part.transform.SetParent(transform);
            part.transform.localPosition = offset;
            part.transform.localEulerAngles = rotation.eulerAngles;
            part.Attach();
            OnPartAttached?.Invoke(part);
        }
        else
        {
            if (!ProjectPreferences.instance.IsAssembly)
            {
                part.DisassemblyInstall();
                return;
            }
            part.transform.SetParent(transform);
            part.transform.localPosition = offset;
            part.transform.localEulerAngles = rotation.eulerAngles;
        }

    }

    private void OnEnable()
    {
        foreach (var p in jointPoints)
        {
            p.OnPartAttached += AttachPart;
            p.gameObject.SetActive(false);
        }
    }
}
