using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StageDisplay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI displayDescText;

    [SerializeField]
    TextMeshProUGUI displayNumText;

    [SerializeField]
    Color failedColor;

    [SerializeField]
    Image bg;

    public void Init(Stage stage, bool error)
    {
        displayDescText.text = stage.description;
        displayNumText.text = stage.ID == 0 ? "" : stage.ID.ToString();
        if (error)
        {
            bg.color = failedColor;
        }
    }
}
