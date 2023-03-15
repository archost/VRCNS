using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Assistant : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField]
    private float preferredDistance;

    private Transform plTransform;

    [SerializeField]
    private bool isFreeFlying;

    [SerializeField]
    private float flyingSpeed;

    [SerializeField]
    private List<Transform> fixedPositions;

    private int posIndex = -1;

    private bool isMoving = false;

    private AudioClip currentClip = null;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        plTransform = Camera.main.transform;
    }

    public void PlayClip(AudioClip clip)
    {        
        if (audioSource.isPlaying) 
        {
            audioSource.Stop();
        }
        currentClip = clip;
        audioSource.clip = clip;
        audioSource.Play();
    }

    [ContextMenu("Next pos")]
    public void NextPos()
    {
        StopAllCoroutines();
        if (posIndex < fixedPositions.Count - 1) 
        {
            posIndex++;
            StartCoroutine(MoveToTarget(fixedPositions[posIndex]));
        }
    }

    private void Update()
    {
        if (!isMoving)
        {
            Quaternion relativeTo = Quaternion.LookRotation((plTransform.position + Vector3.down * 0.7f) - transform.position);
            Quaternion current = transform.localRotation;
            transform.localRotation = Quaternion.Slerp(current, relativeTo, Time.deltaTime);
        }
        if (isFreeFlying) 
        {
            Vector3 dist = transform.position - plTransform.position;
            float distance = dist.magnitude;
            if (distance > preferredDistance)
            {
                dist = dist.normalized;
                dist.y = 0f;
                transform.position = transform.position - 2f * Time.deltaTime * dist;
                //transform.Translate(dist.normalized * Time.deltaTime);
            }
        }        
    }

    IEnumerator MoveToTarget(Transform target)
    {
        isMoving = true;
        if (currentClip != null) yield return new WaitForSeconds(currentClip.length);
        Vector3 dist = target.position - transform.position;
        float distance = dist.magnitude;
        Quaternion oldRotation = transform.localRotation;
        Quaternion lookat = Quaternion.LookRotation(dist);
        float t = 0f;
        float rotationTime = 2f;
        while (t < rotationTime) 
        { 
            transform.localRotation = Quaternion.Lerp(oldRotation, lookat, t / rotationTime);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        } 
        while(distance > 0.1f) 
        {
            dist = dist.normalized;
            dist.y = 0f;
            transform.position = transform.position + flyingSpeed * Time.deltaTime * dist;

            dist = target.position - transform.position;
            distance = dist.magnitude;
            yield return new WaitForEndOfFrame();
        }
        isMoving = false;
    }
}
