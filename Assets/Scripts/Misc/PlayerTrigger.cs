using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActionHandler))]
public class PlayerTrigger : MonoBehaviour
{
    private ActionHandler handler;

    private void Start()
    {
        handler = GetComponent<ActionHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out var p))
        {
            handler.ActionTrigger();
            gameObject.SetActive(false);
        }
    }
}
