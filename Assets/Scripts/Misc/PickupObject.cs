using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActionHandler))]
public class PickupObject : MonoBehaviour
{
    [SerializeField]
    private string objectName = "None";

    private bool isTarget = false;

    private Outline outline;

    private ActionHandler actionHandler;

    private void OnEnable()
    {
        actionHandler = GetComponent<ActionHandler>();
        actionHandler.OnSetTarget += OnSetTarget;
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineColor = ProjectPreferences.instance.outlineColor;
        outline.OutlineWidth = ProjectPreferences.instance.outlineWidth;
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.enabled = false;
    }

    private void OnSetTarget()
    {
        outline.enabled = true;
        isTarget = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isTarget) return;
        if (collision.gameObject.TryGetComponent<Player>(out var p))
        {
            Debug.Log($"Надел {objectName}!");
            p.AddItem(objectName);
            actionHandler.ActionTrigger();
            Destroy(gameObject);
        }
    }
}
