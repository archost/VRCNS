using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ActionHandler))]
public class PlayerTrigger : MonoBehaviour
{
    private ActionHandler handler;

    [SerializeField]
    private UnityEvent triggerEvent;

    private void Start()
    {
        handler = GetComponent<ActionHandler>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out var p))
        {
            handler.ActionTrigger();
            triggerEvent?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
