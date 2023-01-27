using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageDisplay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI displayDescText;

    [SerializeField]
    TextMeshProUGUI displayNumText;

    public void Init(Stage stage)
    {
        displayDescText.text = stage.description;
        displayNumText.text = stage.ID.ToString();
    }
}
