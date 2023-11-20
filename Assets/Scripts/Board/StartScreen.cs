using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    private static Regex s_groupRegex = new Regex("[А-Яа-я0-9]+-[0-9]+", RegexOptions.CultureInvariant);

    [SerializeField]
    private GameObject invalidMessage;

    [SerializeField]
    private TMP_InputField nameField;

    [SerializeField]
    private TMP_InputField surnameField;

    [SerializeField]
    private TMP_InputField groupField;

    [SerializeField]
    private TMP_Dropdown gamemodeDropdown;

    [SerializeField]
    private TMP_Dropdown scenarioDropdown;

    [SerializeField]
    private EventSystem _currentEventSystem;

    private RectTransform messageRect;

    private string validName = "";

    private string validSurname = "";

    private string validGroup = "";

    private GameMode validMode = 0;

    private GameAssemblyType validScenario = 0;

    private bool[] fieldsValid = new bool[3] { false, false, false };

    private void Start()
    {
        if (ModalWindow.WindowPrefab == null)
        {
            ModalWindow.WindowPrefab = Resources.Load<ModalWindow>("Prefabs/ModalWindowContainer");
            if (ModalWindow.WindowPrefab == null)
                throw new MissingReferenceException();
        }

        messageRect = invalidMessage.GetComponent<RectTransform>();

        if (PlayerDataController.instance.IsSoftReset)
        {
            PlayerData pd = PlayerDataController.instance.CurrentPlayerData;
            nameField.SetTextWithoutNotify(pd.PlayerName);
            surnameField.SetTextWithoutNotify(pd.PlayerSurname);
            groupField.SetTextWithoutNotify(pd.Group);
            gamemodeDropdown.SetValueWithoutNotify(pd.GameMode);
            scenarioDropdown.SetValueWithoutNotify(pd.Scenario);
            TrySetName(pd.PlayerName);
            TrySetSurname(pd.PlayerSurname);
            TrySetGroup(pd.Group);
            validMode = (GameMode)pd.GameMode;
            validScenario = (GameAssemblyType)pd.Scenario;
        }
    }

    public void ClearMessage(string s)
    {
        invalidMessage.SetActive(false);
        invalidMessage.transform.position = new Vector3(-100, -100, 0);
    }

    public void TrySetName(string name)
    {
        if (IsNameValid(name))
        {
            nameField.onSelect.RemoveListener(ClearMessage);
            fieldsValid[0] = true;
            validName = name;
        }
        else
        {
            validName = "";
            fieldsValid[0] = false;
            ShowErrorInputMessage(ref nameField, "Неправильно введено имя");
        }
    }

    public void TrySetSurname(string surname)
    {
        if (IsSurnameValid(surname))
        {
            surnameField.onSelect.RemoveListener(ClearMessage);
            fieldsValid[1] = true;
            validSurname = surname;
        }
        else
        {
            validSurname = "";
            fieldsValid[1] = false;
            ShowErrorInputMessage(ref surnameField, "Неправильно введена фамилия");
        }
    }

    public void TrySetGroup(string group)
    {
        if (IsGroupValid(group))
        {
            groupField.onSelect.RemoveListener(ClearMessage);
            fieldsValid[2] = true;
            validGroup = group;
        }
        else
        {
            validGroup = "";
            fieldsValid[2] = false;
            ShowErrorInputMessage(ref groupField, "Неправильно введена группа");
        }

    }

    private void ShowErrorInputMessage(ref TMP_InputField inputField, string message)
    {
        inputField.onSelect.AddListener(ClearMessage);
        var fieldRect = inputField.transform as RectTransform;
        var messagePos = inputField.transform.position +
            (fieldRect.rect.width / 4 + messageRect.rect.width / 4) * inputField.transform.right;
        invalidMessage.SetActive(true);
        invalidMessage.transform.position = messagePos;
        invalidMessage.GetComponentInChildren<TextMeshProUGUI>().text = message;
    }

    public void ShowAbout()
    {
        string title = "О программе";
        string content =
            "Приложение виртуальной реальности \"Виртуальный ремонтный цех\" <br>" +
            "Версия " + Application.version + "<br>" +
            "Данный программный продукт создан командой разработчиков УГНТУ:<br>" +
            "Кафедра ВТИК: Щербаков О.В., Султанов А.И., Хаертдинов И.И., Харисов Х.А.<br>" +
            "Кафедра МОНГП: Мутных Н.В., Любимов К.С.<br>" +
            "Под руководством: Дружинская Е.В., Пензин А.В., Латыпов Б.М.<br>" +
            "Воспроизведение или распространение данной программы или любой её части влечет гражданскую и уголовную ответственность.<br>" +
            "© УГНТУ, 2023 г.";
        Instantiate(ModalWindow.WindowPrefab, transform).ShowInformation(title, content);
    }

    public void ShowServerConnectionError(string errorMessage = "")
    {
        Instantiate(ModalWindow.WindowPrefab, transform).ShowInformation("Ошибка",
            $"Ошибка подключения к серверу.<br>{errorMessage}");
    }

    public void SetGameMode()
    {
        validMode = (GameMode)gamemodeDropdown.value;
    }

    public void SetScenario()
    {
        validScenario = (GameAssemblyType)scenarioDropdown.value;
    }

    private bool IsNameValid(string name)
    {
        return name != "";
    }

    private bool IsSurnameValid(string surname)
    {
        return surname != "";
    }

    private bool IsGroupValid(string group)
    {
        return (group != "" && s_groupRegex.IsMatch(group));
    }

    public void OnExitButtonClicked()
    {
        Instantiate(ModalWindow.WindowPrefab, transform).ShowChoice(
            new Dictionary<string, UnityEngine.Events.UnityAction>()
            {
                {"Назад", null },
                {"Выйти", () => Application.Quit() }
            }, contentText: "Вы уверены, что хотите выйти?", closeButtonVisibility: false);
    }

    [ContextMenu("Test DB")]
    private void TestDB()
    {
        DB_main.instance.CallDB();
    }

    public void NextStage()
    {
        for (int i = 0; i < fieldsValid.Length; i++)
        {
            if (!fieldsValid[i])
            {
                switch (i)
                {
                    case 0:
                        ShowErrorInputMessage(ref nameField, "Неправильно введено имя");
                        break;
                    case 1:
                        ShowErrorInputMessage(ref surnameField, "Неправильно введена фамилия");
                        break;
                    case 2:
                        ShowErrorInputMessage(ref groupField, "Неправильно введена группа");
                        break;
                    default:
                        break;
                }
                return;
            }
        }


        PlayerDataController.instance.SetFirstParameters(validName, validSurname, validGroup, validMode, validScenario);
        ProjectPreferences.instance.assemblyType = validScenario;
        ProjectPreferences.instance.gameMode = validMode;
        Destroy(_currentEventSystem.gameObject);
        SceneLoader.LoadScene(1);
    }
}
