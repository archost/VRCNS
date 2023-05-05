using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StageController))]
public class AdditionalStageActionController : MonoBehaviour
{
    [SerializeField]
    private List<AdditionalAction> actions = new List<AdditionalAction>();

    private List<int> stageIDs = new List<int>();

    private void Awake()
    {
        if (actions.Count == 0)
        {
            Destroy(this);
            return;
        }

        foreach (var item in actions)
        {
            stageIDs.Add(item.stage.ID);
        }

        var sc = GetComponent<StageController>();
        sc.OnStageSwitch += StageCheck;
    }

    private void StageCheck(Stage stage)
    {
        if (stageIDs.Contains(stage.ID))
        {
            var act = actions.Find(x => x.stage.ID == stage.ID);
            act.unityEvent?.Invoke();
        }
    }

    [System.Serializable]
    private struct AdditionalAction
    {
        public Stage stage;
        public UnityEvent unityEvent;
    }
}
