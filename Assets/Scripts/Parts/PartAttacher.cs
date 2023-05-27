using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VREventArgs;

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

    public void PartAttachRequest(PartAttachRequestEventArgs e)
    {
        if (e.ToBeInstalled)
        {
            e.Part.transform.SetParent(transform);
            e.Part.transform.localPosition = e.FixedPosition;
            e.Part.transform.localEulerAngles = e.FixedRotation.eulerAngles;
            e.Part.Attach();
            OnPartAttached?.Invoke(e.Part);
        }
        else
        {
            e.Part.transform.SetParent(transform);
            e.Part.transform.localPosition = e.FixedPosition;
            e.Part.transform.localEulerAngles = e.FixedRotation.eulerAngles;
        }
    }

    private void OnEnable()
    {
        foreach (var p in jointPoints)
        {
            p.OnPartAttachRequest += PartAttachRequest;
            p.gameObject.SetActive(false);
        }
    }
}
