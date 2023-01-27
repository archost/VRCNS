using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickupObject : MonoBehaviour
{
    [SerializeField]
    private string objectName = "None";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out var p))
        {
            Debug.Log($"Надел {objectName}!");
            p.AddItem(objectName);


            Destroy(gameObject);
        }
    }
}
