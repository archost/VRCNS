using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    public RectTransform ContentTransform => _contentTransform;

    public void Show(string titleText = "", string contentText = "")
    {
        _closeButton.onClick.AddListener(Hide);
        if (_blackScreenCanvasGroup == null) _blackScreenCanvasGroup = _blackScreen.GetComponent<CanvasGroup>();
        _titleText.text = titleText;
        _contentText.text = contentText;
        _window.SetActive(true);
        StartCoroutine(FadingScreen(true));
    }

    public void Hide()
    {
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
