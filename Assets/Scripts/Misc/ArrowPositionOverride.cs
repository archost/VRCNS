using UnityEngine;

[RequireComponent(typeof(JointPoint))]
public class ArrowPositionOverride : MonoBehaviour
{
    [SerializeField]
    private Vector3 newArrowPosition = Vector3.zero;

    public Vector3 ArrowPosition 
    { 
        get 
        { 
            return newArrowPosition; 
        } 
    }
}

