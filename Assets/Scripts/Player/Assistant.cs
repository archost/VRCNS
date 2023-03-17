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
    private float deadzone;

    [SerializeField]
    private List<Transform> fixedPositions;

    private int posIndex = -1;

    private bool isMovingBlocked = false;

    private AudioClip currentClip = null;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        plTransform = Camera.main.transform;
    }

    public void PlayClip(AudioClip clip)
    {
        if (!ProjectPreferences.instance.AssistantVoice) return;
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
        if (!isMovingBlocked)
        {
            Quaternion relativeTo = Quaternion.LookRotation((plTransform.position + Vector3.down * 0.7f) - transform.position);
            Quaternion current = transform.localRotation;
            transform.localRotation = Quaternion.Slerp(current, relativeTo, Time.deltaTime);

            if (isFreeFlying)
            {
                Vector3 dist = transform.position - plTransform.position;
                float distance = dist.magnitude;
                if (distance > preferredDistance + deadzone)
                {
                    StopAllCoroutines();
                    StartCoroutine(MoveToTarget(plTransform, preferredDistance));
                }
            }
        }
               
    }

    IEnumerator MoveToTarget(Transform target, float prefDistance = 0.1f)
    {
        isMovingBlocked = true;        
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
        if (audioSource.isPlaying) yield return new WaitForSeconds(currentClip.length);
        while (distance > prefDistance) 
        {
            dist = dist.normalized;
            dist.y = 0f;
            transform.position = transform.position + flyingSpeed * Time.deltaTime * dist;

            dist = target.position - transform.position;
            distance = dist.magnitude;
            yield return new WaitForEndOfFrame();
        }
        isMovingBlocked = false;
    }
}
