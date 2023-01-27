using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PartHelper : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI partName;

    [SerializeField]
    private TextMeshProUGUI partDesc;

    [SerializeField]
    private float heightOffset = 1.5f;

    private Transform pl;

    private Transform currentTarget = null;

    private void Start()
    {
        pl = FindObjectOfType<Player>().transform;
    }

    public void SetTarget(Transform target, PartData data)
    {
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        currentTarget = target;
        partName.text = data.PartName;
        partDesc.text = data.Description;
        transform.position = currentTarget.position + Vector3.up * heightOffset;
        transform.LookAt(pl);
    }

    public void TurnOff()
    {
        currentTarget = null;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (currentTarget != null) 
        {
            transform.position = currentTarget.position + Vector3.up * heightOffset;
            transform.LookAt(pl);
        }
    }
}
