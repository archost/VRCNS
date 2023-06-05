using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioController))]
public class Player : MonoBehaviour
{
    private List<string> Items = new List<string>();

    [SerializeField]
    private List<string> expectedItems = new List<string>();

    [SerializeField]
    private UnityEvent OnReady;

    public string PlayerName { get; private set; }

    public string PlayerGroup { get; private set; }

    public bool IsReady { get; private set; }

    private AudioController ac;

    private void Start()
    {
        IsReady = false;
        //wboard.OnFieldSubmit += CheckReady;
        ac = GetComponent<AudioController>();
    }

    [ContextMenu("Equip Everything")]
    public void Test()
    {
        foreach (var item in expectedItems)
        {
            AddItem(item);
        }
        IsReady = true;
        //wboard.GetFieldsValues(out string s1, out string s2);
        //PlayerName = s1;
        //PlayerGroup = s2;
        OnReady?.Invoke();
    }

    public void AddItem(string item)
    {
        Items.Add(item);
        ac.PlayClip("equip");
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

    public void PlaySound(string clip)
    {
        ac.TryPlayClip(clip);
    }

    private void CheckReady()
    {
        if (IsReady) return;
        //if (IsFullyEquipped() && wboard.IsFieldsValid)
        if (IsFullyEquipped())
        {
            IsReady = true;
            //wboard.GetFieldsValues(out string s1, out string s2);
            //PlayerName = s1;
            //PlayerGroup = s2;
            OnReady?.Invoke();
        }
    }
}
