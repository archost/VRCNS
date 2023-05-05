using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastLine : MonoBehaviour
{
    private LineRenderer laserline;

    private void Start()
    {
        laserline = GetComponent<LineRenderer>();
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 3))
        {
            laserline.enabled = true;
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green);
            laserline.SetPosition(0, transform.position);
            laserline.SetPosition(1, hit.point);
        }
        else
        {
            laserline.enabled = false;
        }
    }
}
