using UnityEngine;
using TMPro;

public class WelcomingBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameField;

    [SerializeField]
    private TextMeshProUGUI groupField;

    [SerializeField]
    private TMP_Dropdown gamemodeDropdown;

    [SerializeField]
    private TMP_Dropdown assemblyDropdown;

    private void Start()
    {
        PlayerData pld = PlayerDataController.instance.CurrentPlayerData;
        nameField.text = pld.PlayerName;
        groupField.text = pld.Group;
        gamemodeDropdown.value = pld.GameMode;
        assemblyDropdown.value = pld.Scenario;
    }

}
