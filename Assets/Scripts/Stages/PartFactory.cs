using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class PartFactory : MonoBehaviour
{
    [SerializeField]
    private List<SpawnInfo> spawnInfos = new List<SpawnInfo>();

    public List<PartAttacher> partAttachers;

    public List<Part> partsQueue = new List<Part>();

    public bool IsDone { get; private set; }

    private void OnEnable()
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

    public void ProceedQueue()
    {
        if (partsQueue.Count == 0) return;
        foreach (var part in partsQueue)
        {
            bool succeed = false;
            foreach (var attacher in partAttachers)
            {
                var jp = attacher.GetJointPointByPartID(true, part.PartID);
                if (jp != null)
                {
                    jp.ForceAttach(part, true);
                    succeed = true;
                }
            }
            if (!succeed)
            {
                Debug.LogWarning("Attacher was not found for this Part", part.gameObject);
            }
        }
        partsQueue.Clear();
    }

    public IEnumerator SpawnParts(Mediator mediator, List<SpawnInfo> spawns)
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
            RegisterPart(s, p, mediator, false);
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

    private void RegisterPart(SpawnInfo s, Part p, Mediator mediator, bool t)
    {
        var presenter = p.InitPartPresenter(mediator);
        mediator.AddPart(presenter);
        partAttachers.Add(p.GetComponent<PartAttacher>());
        if (s.point == null)
        {
            partsQueue.Add(p);
        }
        else s.point.ForceAttach(p, t);
    }
}

[System.Serializable]
public struct SpawnInfo
{
    public Part partPrefab;
    public JointPoint point;
}