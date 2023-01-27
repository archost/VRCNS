using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPart", menuName = "Part")]
public class PartData : ScriptableObject
{
    public int ID;

    public string PartName;

    public string Description;
}
