using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdminPanel : MonoBehaviour
{
    private string _password = null;

    [SerializeField]
    private TMP_InputField _authPasswordField;

    [SerializeField]
    private GameObject _authPanel;

    [SerializeField]
    private TMP_InputField _apiField;

    private const string ADMINPASS_KEY = "adminPass";

    private void Awake()
    {
        if (_password == null)
        {
            if (PlayerPrefs.HasKey(ADMINPASS_KEY))
            {
                _password = PlayerPrefs.GetString(ADMINPASS_KEY);
            }
            else
            {
                (_authPasswordField.placeholder as TMP_Text).text = "¬ведите новый пароль...";
            }
        }
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    public void OnBackButton()
    {
        _authPasswordField.gameObject.SetActive(true);
        _authPanel.SetActive(false);
        gameObject.SetActive(false);
    }

    public void SubmitPassword(string value)
    {
        if (_password == null)
        {
            var pass = Encoder.EncodeString(value);
            _password = Encoder.ByteToString(pass);
            PlayerPrefs.SetString(ADMINPASS_KEY, _password);
            _authPasswordField.SetTextWithoutNotify(string.Empty);
            AuthValidated();
        }
        else
        {
            string val = Encoder.ByteToString(Encoder.EncodeString(value));
            if (_password.Equals(val))
            {
                _authPasswordField.SetTextWithoutNotify(string.Empty);
                AuthValidated();
            }    
        }
    }

    private void AuthValidated()
    {
        _authPasswordField.gameObject.SetActive(false);
        _authPanel.SetActive(true);
        if (PlayerPrefs.HasKey(ProjectPreferences.BackendAddressKey))
        {
            _apiField.SetTextWithoutNotify(PlayerPrefs.GetString(ProjectPreferences.BackendAddressKey));
        }
        else
        {
            _apiField.SetTextWithoutNotify("none");
        }
    }

    public void SubmitAPI(string value)
    {
        PlayerPrefs.SetString(ProjectPreferences.BackendAddressKey, value);
    }
}
