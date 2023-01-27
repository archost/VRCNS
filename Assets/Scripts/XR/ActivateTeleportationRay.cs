using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ActivateTeleportationRay : MonoBehaviour
{
    public GameObject rightTeleportation;
    public GameObject leftTeleportation;

    public InputActionProperty rightActivate;
    public InputActionProperty leftActivate;

    public InputActionProperty leftCancel;
    public InputActionProperty rightCancel;

    public XRRayInteractor leftRay;
    public XRRayInteractor rightRay;

    void Update()
    {
        //bool isRightHovering = HandRayController.instance.IsRightHovering;
        bool isRightHovering = rightRay.TryGetHitInfo(out Vector3 RightPos, out Vector3 RightNormal, out int RightNumber, out bool RightValid);
        
        rightTeleportation.SetActive(!isRightHovering && rightCancel.action.ReadValue<float>() == 0 && rightActivate.action.ReadValue<float>() > 0.1f);

        //bool isLeftHovering = HandRayController.instance.IsLeftHovering;
        bool isLeftHovering = leftRay.TryGetHitInfo(out Vector3 leftPos, out Vector3 leftNormal, out int leftNumber, out bool leftValid);

        leftTeleportation.SetActive(!isLeftHovering && leftCancel.action.ReadValue<float>() == 0f && leftActivate.action.ReadValue<float>() > 0.1f);
    }
}
