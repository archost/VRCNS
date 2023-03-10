using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName ="NewStage", menuName ="Stage")]
public class Stage : ScriptableObject
{
    public int ID;

    public string description;

    public StageGoalType goalType;

    public PartData target;

    public PartState initPartState = PartState.Idle;

    public string actionCode = string.Empty;

#if UNITY_EDITOR
    [CustomEditor(typeof(Stage))]
    public class StageEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            Stage stage = (Stage)target;

            EditorGUILayout.LabelField("Stage properties:");
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Stage ID", GUILayout.MaxWidth(75));
            stage.ID = EditorGUILayout.IntField(stage.ID, GUILayout.MaxWidth(40));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Description");
            stage.description = EditorGUILayout.TextArea(stage.description, GUILayout.Height(80));

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Stage Goal", GUILayout.MaxWidth(75));
            stage.goalType = (StageGoalType)EditorGUILayout.EnumPopup(stage.goalType);
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            if (stage.goalType == StageGoalType.PartPlacing)
            {
                stage.target = EditorGUILayout.ObjectField("Target Part", stage.target, typeof(PartData), false) as PartData;

                stage.initPartState = (PartState)EditorGUILayout.EnumPopup("Init Part State", stage.initPartState);
            }
            else if (stage.goalType == StageGoalType.Action)
            {
                stage.actionCode = EditorGUILayout.TextField("Action code", stage.actionCode);
            }
            EditorGUI.indentLevel--;
            EditorUtility.SetDirty(target);
        }
    }
#endif
}

public enum StageGoalType
{
    PartPlacing,
    Action
}
