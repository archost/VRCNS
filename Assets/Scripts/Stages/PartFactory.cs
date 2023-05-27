using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VREventArgs;

public class PartFactory : MonoBehaviour
{
    [SerializeField]
    private List<SpawnInfo> spawnInfos = new List<SpawnInfo>();

    [SerializeField]
    private List<PartAttacher> partAttachers;

    private List<Part> parts = new List<Part>();

    public bool IsDone { get; private set; }

    private void Awake()
    {
        partAttachers = FindObjectsOfType<PartAttacher>().ToList();
    }

    public void ToogleSuitablePoints(GameAssemblyType assemblyType, PartData data)
    {
        for (int i = 0; i < partAttachers.Count; i++)
        {
            if (partAttachers[i].TryToogleJointPointByPart(assemblyType == GameAssemblyType.Assembly, data))
            {
                Debug.Log($"Enabled JointPoint in {partAttachers[i].name}");
            }
        }
    }

    public void SetPartAsTarget(TargetSetEventArgs e)
    {
        var args = e as PartSetAsTargetEventArgs;
        var pd = args.TargetData.ID;
        foreach (var part in parts)
        {
            if (part.PartID == pd)
            {
                part.SetAsTarget(e);
            }
        }
    }

    public IEnumerator SpawnParts(List<SpawnInfo> spawns)
    {
        IsDone = false;
        var init = new Initialization();
        spawnInfos.Clear();
        spawnInfos = spawns;
        int totalProgress = spawns.Count;
        int k = 0;
        foreach (var s in spawnInfos)
        {
            Part p = Instantiate(s.partPrefab);
            RegisterPart(s, p, true, false);
            var state = s.spawnState;
            if (state == PartState.Installed) p.Install(silent: true);
            k++;
            
            var childParts = p.gameObject.GetComponentsInChildren<Part>();
            if (childParts.Length > 1)
            {
                totalProgress += childParts.Length - 1;
                if (state == PartState.Installed) foreach (var child in childParts) child.Install(silent: true);
                foreach (var child in childParts)
                {
                    if (child == p) continue;
                    RegisterPart(s, child, false);
                    k++;
                    init.Progress = (float)k / totalProgress;
                    yield return new WaitForEndOfFrame();
                }
            }
            
            init.Progress = (float)k / totalProgress;
            yield return new WaitForEndOfFrame();
        }
        IsDone = true;
        Debug.Log($"Spawn complete! ({totalProgress} instances)");
    }

    private void RegisterPart(SpawnInfo spawnInfo, Part part, bool changePartPosition = false, bool toBeInstalled = false)
    {
        parts.Add(part);
        partAttachers.Add(part.GetComponent<PartAttacher>());
        if (changePartPosition)
            spawnInfo.point.ForceAttach(part, toBeInstalled);
    }
}

[System.Serializable]
public struct SpawnInfo
{
    public Part partPrefab;
    public JointPoint point;
    public PartState spawnState;
}