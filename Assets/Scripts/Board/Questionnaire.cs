using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Events;
using VREventArgs;

public class Questionnaire : MonoBehaviour
{
    public event UnityAction OnQuestionAnswered;

    [SerializeField]
    private TextMeshProUGUI questionText;

    [SerializeField]
    private Button[] optionsButton;

    [SerializeField]
    private bool multiErrorAllowed;

    [SerializeField]
    private float height;

    private TextMeshProUGUI[] optionsText;

    [SerializeField]
    private Color mistakeColor;

    [SerializeField]
    private Color correctColor;

    private Transform pl = null;

    private Question currentQuestion = null;

    private AudioController ac;

    private void Awake()
    {
        ac = GetComponent<AudioController>();
        pl = Camera.main.transform;

        multiErrorAllowed = ProjectPreferences.instance.IsTraining;

        optionsText = new TextMeshProUGUI[optionsButton.Length];
        for (int i = 0; i < optionsButton.Length; i++)
        {
            optionsText[i] = optionsButton[i].gameObject.GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void SetPosition(Vector3 position)
    {
        position.y = height;
        transform.position = position;
    }

    public void SetQuestion(Question q)
    {
        if (q.options.Length > 4) throw new System.Exception(q.name + " question has wrong options length!");
        OnQuestionAnswered = null;
        gameObject.SetActive(true);
        currentQuestion = q;
        DisableAllButtons();
        ResetButtonsColor();
        ResetButtonsText();
        questionText.text = q.questionDescription;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < q.options.Length; i++)
        {
            sb.Clear();
            sb.Append((char)('A' + i));
            sb.Append(". ");
            sb.Append(q.options[i]);
            optionsButton[i].interactable = true;
            optionsButton[i].image.color = Color.white;
            optionsText[i].text = sb.ToString();

        }
        ac.PlayClip("notification");
    }

    public void ChooseOption(int index)
    {
        if (currentQuestion == null) return;
        if (index == currentQuestion.correctOption)
        {
            DisableAllButtons();
            foreach (var button in optionsButton)
            {
                if (button.image.color == Color.white) button.image.color = button.colors.normalColor;
            }
            optionsButton[index].image.color = correctColor;
        }
        else 
        {
            StageController.OnMadeMistake.Invoke(new QuestionnaireEventArgs(this, currentQuestion, index));            
            if (multiErrorAllowed)
            {
                optionsButton[index].interactable = false;
                optionsButton[index].image.color = mistakeColor;
                return;
            }
            else
            {
                DisableAllButtons();
                ResetButtonsColor();
                optionsButton[index].interactable = false;
                optionsButton[index].image.color = mistakeColor;
                optionsButton[currentQuestion.correctOption].image.color = correctColor;
            }
        }
        OnQuestionAnswered?.Invoke();
        Invoke(nameof(Hide), 3f);
    }

    private void Update()
    {
        transform.LookAt(pl);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        currentQuestion = null;
    }

    private void DisableAllButtons()
    {
        for (int i = 0; i < optionsButton.Length; i++)
        {
            optionsButton[i].interactable = false;
        }
    }
    
    private void ResetButtonsColor()
    {
        for (int i = 0; i < optionsButton.Length; i++)
        {
            optionsButton[i].image.color = optionsButton[i].colors.normalColor;
        }
    }

    private void ResetButtonsText()
    {
        for (int i = 0; i < optionsButton.Length; i++)
        {
            optionsText[i].text = "";
        }
    }
}
