using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject invalidMessage;

    [SerializeField]
    private TMP_InputField nameField;

    [SerializeField]
    private TMP_InputField groupField;

    [SerializeField]
    private TMP_Dropdown gamemodeDropdown;

    [SerializeField]
    private TMP_Dropdown scenarioDropdown;

    [SerializeField]
    private EventSystem _currentEventSystem;

    private Vector3 nameFieldRightPos;

    private Vector3 groupFieldRightPos;

    private RectTransform nameFieldRect;

    private RectTransform groupFieldRect;

    private RectTransform messageRect;

    private string validName = "";

    private string validGroup = "";

    private GameMode validMode = 0;

    private GameAssemblyType validScenario = 0;

    private bool[] fieldsValid = new bool[2] { false, false };

    private void Start()
    {
        if (ModalWindow.WindowPrefab == null)
        {
            ModalWindow.WindowPrefab = Resources.Load<ModalWindow>("Prefabs/ModalWindowContainer");
            if (ModalWindow.WindowPrefab == null)
                throw new MissingReferenceException();
        }

        nameFieldRect = nameField.GetComponent<RectTransform>();
        groupFieldRect = groupField.GetComponent<RectTransform>();
        messageRect = invalidMessage.GetComponent<RectTransform>();

        if (PlayerDataController.instance.IsSoftReset)
        {
            PlayerData pd = PlayerDataController.instance.CurrentPlayerData;
            nameField.SetTextWithoutNotify(pd.PlayerName);
            groupField.SetTextWithoutNotify(pd.Group);
            gamemodeDropdown.SetValueWithoutNotify(pd.GameMode);
            scenarioDropdown.SetValueWithoutNotify(pd.Scenario);
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
            nameField.onSelect.AddListener(ClearMessage);
            nameFieldRightPos = nameField.transform.position +
                (nameFieldRect.rect.width / 4 + messageRect.rect.width / 4) * nameField.transform.right;
            invalidMessage.SetActive(true);
            invalidMessage.transform.position = nameFieldRightPos;
            invalidMessage.GetComponentInChildren<TextMeshProUGUI>().text = "����������� ������� ���";
        }
    }

    public void TrySetGroup(string group)
    {
        if (IsGroupValid(group))
        {
            groupField.onSelect.RemoveListener(ClearMessage);
            fieldsValid[1] = true;
            validGroup = group;
        }
        else
        {
            validGroup = "";
            fieldsValid[1] = false;
            groupField.onSelect.AddListener(ClearMessage);
            groupFieldRightPos = groupField.transform.position +
                (groupFieldRect.rect.width / 4 + messageRect.rect.width / 4) * groupField.transform.right;
            invalidMessage.SetActive(true);
            invalidMessage.transform.position = groupFieldRightPos;
            invalidMessage.GetComponentInChildren<TextMeshProUGUI>().text = "����������� ������� ������";
        }

    }

    public void ShowAbout()
    {
        string title = "� ���������";
        string content = 
            "���������� ����������� ���������� \"����������� ��������� ���\" (���. " + ProjectPreferences.version + ")<br>" +
            "������ ����������� ������� ������ �������� ������������� �����:<br>" +
            "������� ����: �������� �.�., �������� �.�., ���������� �.�., ������� �.�.<br>" +
            "������� �����: ������ �.�., ������� �.�.<br>" +
            "��� ������������: ���������� �.�., ������ �.�., ������� �.�.<br>" +
            "��������������� ��� ��������������� ������ ��������� ��� ����� � ����� ������ ����������� � ��������� ���������������.<br>" +
            "� �����, 2023 �.";
        Instantiate(ModalWindow.WindowPrefab, transform).ShowInformation(title, content);
    }

    public void ShowServerConnectionError(string errorMessage = "")
    {
        Instantiate(ModalWindow.WindowPrefab, transform).ShowInformation("������",
            $"������ ����������� � �������.<br>{errorMessage}");
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

    private bool IsGroupValid(string group)
    {
        return group != "";
    }

    public void OnExitButtonClicked()
    {
        Instantiate(ModalWindow.WindowPrefab, transform).ShowChoice(
            new Dictionary<string, UnityEngine.Events.UnityAction>()
            {
                {"�����", null },
                {"�����", () => Application.Quit() }
            }, contentText: "�� �������, ��� ������ �����?", closeButtonVisibility: false);
    }

    [ContextMenu("Test DB")]
    private void TestDB()
    {
        DB_main.instance.CallDB();
    }

    public void NextStage()
    {

        foreach (var item in fieldsValid)
        {
            if (!item) return;
        }

        PlayerDataController.instance.SetFirstParameters(validName, validGroup, validMode, validScenario);
        ProjectPreferences.instance.assemblyType = validScenario;
        ProjectPreferences.instance.gameMode = validMode;
        Destroy(_currentEventSystem.gameObject);
        SceneLoader.LoadScene(1);
    }
}
