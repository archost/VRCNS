using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;

public class PartHelper : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI partName;

    [SerializeField]
    private TextMeshProUGUI partDesc;

    [SerializeField]
    private float heightOffset = 1.5f;

    [SerializeField]
    private float distanceOffset = 0.5f;

    [SerializeField]
    private VideoPlayer vplayer;

    private Transform pl;

    private Transform currentTarget = null;

    private void Start()
    {
        pl = Camera.main.transform;
    }

    public void SetTarget(Transform target, PartData data)
    {
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        currentTarget = target;
        partName.text = data.PartName;
        partDesc.text = data.Description;
        if (data.videoClip != null) 
        {
            vplayer.clip = data.videoClip;
            vplayer.gameObject.SetActive(true);
        }
        transform.position = currentTarget.position + Vector3.up * heightOffset;
        transform.LookAt(pl);
        transform.Translate(distanceOffset * (-transform.forward));
    }

    public void TurnOff()
    {
        currentTarget = null;
        vplayer.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (currentTarget != null) 
        {
            transform.position = currentTarget.position + Vector3.up * heightOffset;
            transform.LookAt(pl);
            transform.Translate(distanceOffset * (-transform.forward));
        }
    }
}
