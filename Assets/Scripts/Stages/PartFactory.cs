using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PartFactory : MonoBehaviour
{
    [SerializeField]
    private List<SpawnInfo> spawnInfos = new List<SpawnInfo>();

    public List<PartAttacher> partAttachers;

    private void OnEnable()
    {
        partAttachers = FindObjectsOfType<PartAttacher>().ToList();
    }

    public void ToogleSuitablePoints(PartData data)
    {
        for (int i = 0; i < partAttachers.Count; i++)
        {
            if (partAttachers[i].TryToogleJointPointByPart(ProjectPreferences.instance.assemblyType == GameAssemblyType.Assembly, data))
            {
                Debug.Log($"Enabled JointPoint in {partAttachers[i].name}");
            }
        }
    }

    public void SpawnParts(Mediator mediator, List<SpawnInfo> spawns)
    {
        spawnInfos.Clear();
        spawnInfos = spawns;
        foreach (var s in spawnInfos)
        {
            Part p = Instantiate(s.partPrefab);
            var presenter = p.InitPartPresenter(mediator);
            mediator.AddPart(presenter);
            partAttachers.Add(p.GetComponent<PartAttacher>());
            s.point.ForceAttach(p, false);
        }
        Debug.Log($"Spawn complete! ({spawnInfos.Count} instances)");
    }



}

[System.Serializable]
public struct SpawnInfo
{
    public Part partPrefab;
    public JointPoint point;
}