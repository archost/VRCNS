using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelChanger : MonoBehaviour
{
    [SerializeField]
    private GameObject[] defaults;

    private int index = -1;

    private void Start()
    {
        ToogleNextModel();
    }

    public void ToogleNextModel()
    {
        if (index + 1 >= defaults.Length) return;
        if (index >= 0) defaults[index].SetActive(false);
        index++;
        defaults[index].SetActive(true);
    }
}
