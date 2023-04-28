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
        int k = 0;
        foreach (var s in spawnInfos)
        {
            Part p = Instantiate(s.partPrefab);
            var presenter = p.InitPartPresenter(mediator);
            mediator.AddPart(presenter);
            partAttachers.Add(p.GetComponent<PartAttacher>());
            if (s.point == null)
            {
                partsQueue.Add(p);
            }
            else s.point.ForceAttach(p, false);
            k++;
            init.Progress = (float)k / spawns.Count;
            yield return new WaitForEndOfFrame();
        }
        IsDone = true;
        Debug.Log($"Spawn complete! ({spawnInfos.Count} instances)");
    }
}

[System.Serializable]
public struct SpawnInfo
{
    public Part partPrefab;
    public JointPoint point;
}