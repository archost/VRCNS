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
            RegisterPart(s, p, false);
            k++;
            /*
            var childParts = p.gameObject.GetComponentsInChildren<Part>();
            if (childParts.Length > 0)
            {
                totalProgress += childParts.Length;
                foreach (var child in childParts)
                {
                    RegisterPart(s, child, mediator, false);
                    k++;
                    init.Progress = (float)k / totalProgress;
                    yield return new WaitForEndOfFrame();
                }
            }
            */
            init.Progress = (float)k / totalProgress;
            yield return new WaitForEndOfFrame();
        }
        IsDone = true;
        Debug.Log($"Spawn complete! ({totalProgress} instances)");
    }

    private void RegisterPart(SpawnInfo s, Part p, bool t)
    {
        parts.Add(p);
        partAttachers.Add(p.GetComponent<PartAttacher>());
        s.point.ForceAttach(p, t);
    }
}

[System.Serializable]
public struct SpawnInfo
{
    public Part partPrefab;
    public JointPoint point;
}