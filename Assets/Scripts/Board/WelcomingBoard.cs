using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRKeys;
using TMPro;
using UnityEngine.Events;

public class WelcomingBoard : MonoBehaviour
{
    [SerializeField]
    private Keyboard keyboard;

    [SerializeField]
    private TextMeshProUGUI NameField;

    [SerializeField]
    private TextMeshProUGUI NamePlaceholder;

    [SerializeField]
    private TextMeshProUGUI GroupField;

    [SerializeField]
    private TextMeshProUGUI GroupPlaceholder;

    [SerializeField]
    private GameObject[] selectors;

    [SerializeField]
    private GameObject[] checkers;

    private int toogledField = -1;

    public UnityAction OnFieldSubmit;

    public bool IsFieldsValid
    {
        get
        {
            foreach (var item in checkers)
            {
                if (!item.activeSelf) return false;
            }
            return true;
        }
        
    }

    private void OnEnable()
    {
        keyboard.OnSubmit.AddListener(OnSubmitField);
        keyboard.OnCancel.AddListener(OnCancelField);
    }

    public void ToogleField(int value)
    {
        toogledField = value == 0 || value == 1 ? value : -1;
        if (keyboard.disabled)
        {
            keyboard.Enable();
        }

        checkers[value].SetActive(false);
        foreach (var item in selectors)
        {
            item.SetActive(false);
        }
        selectors[value].gameObject.SetActive(true);
        switch (value)
        {
            case 0:

                keyboard.displayText = NameField;
                keyboard.placeholder = NamePlaceholder;
                break;
            case 1:
                keyboard.displayText = GroupField;
                keyboard.placeholder = GroupPlaceholder;
                break;
            default:
                break;
        }
    }

    public void GetFieldsValues(out string name, out string group)
    {
        name = NameField.text;
        group = GroupField.text;
    }

    private void OnSubmitField(string value)
    {
        bool isValid = toogledField == 0 ? NameFieldValidation(value) : toogledField == 1 && GroupFieldValidation(value);

        if (isValid) 
        {
            keyboard.ShowSuccessMessage("Сохранено!");
            foreach (var item in selectors)
            {
                item.SetActive(false);
            }
            checkers[toogledField].SetActive(true);
            keyboard.text = "";
            OnFieldSubmit?.Invoke();
            keyboard.Disable();
            toogledField = -1;            
            Invoke(nameof(HideMessages), 2f);
        }
        else
        {
            keyboard.ShowValidationMessage("Неправильно введены данные!");
            Invoke(nameof(HideMessages), 2f);
        }
    }

    private void OnCancelField()
    {
        foreach (var item in selectors)
        {
            item.SetActive(false);
        }
        toogledField = -1;
    }

    private void HideMessages()
    {
        keyboard.HideSuccessMessage();
        keyboard.HideValidationMessage();
    }

    private bool NameFieldValidation(string value)
    {
        return value != "";
    }

    private bool GroupFieldValidation(string value) 
    {
        return value != "";
    }
}
