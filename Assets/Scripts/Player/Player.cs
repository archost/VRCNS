using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    private List<string> Items = new List<string>();

    [SerializeField]
    private List<string> expectedItems = new List<string>();

    [SerializeField]
    private UnityEvent OnReady;

    [SerializeField]
    private WelcomingBoard wboard;

    public string PlayerName { get; private set; }

    public string PlayerGroup { get; private set; }

    public bool IsReady { get; private set; }

    private AudioSource a_s;

    private void Start()
    {
        IsReady = false;
        wboard.OnFieldSubmit += CheckReady;
        a_s = GetComponent<AudioSource>();
    }

    [ContextMenu("Equip Everything")]
    public void Test()
    {
        foreach (var item in expectedItems)
        {
            AddItem(item);
        }
    }

    public void AddItem(string item)
    {
        Items.Add(item);
        a_s.Play();
        CheckReady();
    }

    private bool IsFullyEquipped()
    {
        foreach (var item in expectedItems)
        {
            if (!Items.Contains(item))
            {
                return false;
            }
        }
        return true;
    }

    private void CheckReady()
    {
        if (IsReady) return;
        if (IsFullyEquipped() && wboard.IsFieldsValid)
        {
            IsReady = true;
            wboard.GetFieldsValues(out string s1, out string s2);
            PlayerName = s1;
            PlayerGroup = s2;
            OnReady?.Invoke();
        }
    }
}
