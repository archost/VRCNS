using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModalWindow : MonoBehaviour
{
    public static ModalWindow WindowPrefab;

    [SerializeField]
    private float _fadingTime = 1f;

    [SerializeField]
    private GameObject _blackScreen;

    [SerializeField]
    private GameObject _window;

    private CanvasGroup _blackScreenCanvasGroup;

    [SerializeField]
    private TMP_Text _titleText;

    [SerializeField]
    private TMP_Text _contentText;

    [SerializeField]
    private Button _closeButton;

    [SerializeField]
    private RectTransform _contentTransform;

    [SerializeField]
    private RectTransform _buttonContainer;

    [SerializeField]
    private Button _additionalButtonPrefab;

    private List<Button> _additionalButtons;

    public RectTransform ContentTransform => _contentTransform;

    public void ShowInformation(string titleText = "", string contentText = "")
    {
        _closeButton.onClick.AddListener(Hide);
        if (_blackScreenCanvasGroup == null) _blackScreenCanvasGroup = _blackScreen.GetComponent<CanvasGroup>();
        _titleText.text = titleText;
        _contentText.text = contentText;
        _window.SetActive(true);
        StartCoroutine(FadingScreen(true));
    }

    public void ShowChoice(Dictionary<string, UnityAction> buttonsNamesAndCallbacks, string titleText = "", 
        string contentText = "", bool closeButtonVisibility = true)
    {
        if (closeButtonVisibility) 
            _closeButton.onClick.AddListener(Hide);
        else
            _closeButton.gameObject.SetActive(false);
        _additionalButtons = new List<Button>();
        foreach (var item in buttonsNamesAndCallbacks)
        {
            var button = Instantiate(_additionalButtonPrefab, _buttonContainer);
            button.GetComponentInChildren<TMP_Text>().text = item.Key;
            if (item.Value == null)
                button.onClick.AddListener(Hide);
            else
                button.onClick.AddListener(item.Value);
            button.gameObject.SetActive(true);
            _additionalButtons.Add(button);
        }
        if (_blackScreenCanvasGroup == null) _blackScreenCanvasGroup = _blackScreen.GetComponent<CanvasGroup>();
        _titleText.text = titleText;
        _contentText.text = contentText;
        _window.SetActive(true);
        StartCoroutine(FadingScreen(true));
    }

    public void Hide()
    {
        if (_additionalButtons?.Count > 0)
        {
            foreach (var item in _additionalButtons)
            {
                Destroy(item.gameObject);
            }
            _additionalButtons.Clear();
        }
        _closeButton.onClick.RemoveListener(Hide);
        _window.SetActive(false);
        StartCoroutine(FadingScreen(false));
    }

    private IEnumerator FadingScreen(bool show)
    {
        float targetAlpha = 0f;
        if (show)
        {
            targetAlpha = 0.7f;
        }
        else
            targetAlpha = 0f;
        float timer = 0f;
        if (show) _blackScreen.SetActive(show);
        float currentAlpha = _blackScreenCanvasGroup.alpha;
        while (timer < _fadingTime)
        {            
            timer += Time.deltaTime;
            _blackScreenCanvasGroup.alpha = Mathf.Lerp(currentAlpha, targetAlpha, timer / _fadingTime);
            yield return new WaitForEndOfFrame();
        }
        if (!show)
        {
            _blackScreen.SetActive(show);
            Destroy(gameObject);
        }
    }
}
