using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class NextBot : MonoBehaviour
{
    private Transform player;

    [SerializeField]
    private float flyingSpeed;

    [SerializeField]
    private UnityEvent collideAction;

    private void OnEnable()
    {
        player = Camera.main.transform;
    }

    private void Update()
    {
        if (player != null)
        {
            Quaternion relativeTo = Quaternion.LookRotation((player.position + Vector3.down * 0.7f) - transform.position);
            Quaternion current = transform.localRotation;
            transform.localRotation = Quaternion.Slerp(current, relativeTo, Time.deltaTime);
            Vector3 dist = player.position - transform.position;
            float distance = dist.magnitude;
            while (distance > 0.1f)
            {
                dist = dist.normalized;
                dist.y = 0f;
                transform.position = transform.position + flyingSpeed * Time.deltaTime * dist;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<Player>(out var p))
        {
            collideAction?.Invoke();
        }
    }
}
