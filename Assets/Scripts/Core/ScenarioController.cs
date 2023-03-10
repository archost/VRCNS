using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    [SerializeField]
    private Scenario assemblyScenario;

    [SerializeField]
    private Scenario disassemblyScenario;

    public Scenario GetScenario()
    {
        return ProjectPreferences.instance.assemblyType switch
        {
            GameAssemblyType.Assembly => assemblyScenario,
            GameAssemblyType.Disassembly => disassemblyScenario,
            _ => new Scenario(),
        };
    }
}

[System.Serializable]
public struct Scenario
{
    public List<Stage> stagesList;
    public List<SpawnInfo> spawnList;
}
