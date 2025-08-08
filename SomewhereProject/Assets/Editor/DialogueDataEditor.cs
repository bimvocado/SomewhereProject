#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueData))]
public class DialogueDataEditor : Editor
{
    private SerializedProperty dialoguesProp;
    private SerializedProperty nextDialogueOnCompletionProp;

    private void OnEnable()
    {
        dialoguesProp = serializedObject.FindProperty("dialogues");
        nextDialogueOnCompletionProp = serializedObject.FindProperty("nextDialogueOnCompletion");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Dialogue Data 에디터", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(nextDialogueOnCompletionProp, new GUIContent("완료 후 다음 대화"));
        EditorGUILayout.HelpBox("현재 대화가 끝나고 자동으로 재생될 다음 DialogueData 지정(addressable data)\ncsv에서 지정했다면 자동 등록 / 선택지의 경우 등록 안해도 ㄱㅊ", MessageType.Info);
        EditorGUILayout.Space();


        EditorGUILayout.PropertyField(dialoguesProp, new GUIContent("line list"), true);

        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif