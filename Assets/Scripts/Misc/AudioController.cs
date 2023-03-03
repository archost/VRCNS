using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    private AudioSource source;

    [SerializeField]
    private List<AudioClipElement> clipElements;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayClip(string key)
    {
        if (clipElements.Count == 0)
        {
            Debug.LogError("AudioController's Clip List is empty!", gameObject);
            return;
        }
        foreach (var item in clipElements)
        {
            if(item.key == key)
            {
                source.PlayOneShot(item.clip);
                return;
            }
        }
        Debug.LogError($"Clip \"{key}\" was not found!");
    }

    [System.Serializable]
    private struct AudioClipElement
    {
        public string key;
        public AudioClip clip;
    }
}
