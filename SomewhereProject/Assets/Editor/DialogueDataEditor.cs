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

        EditorGUILayout.LabelField("Dialogue Data ������", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(nextDialogueOnCompletionProp, new GUIContent("�Ϸ� �� ���� ��ȭ"));
        EditorGUILayout.HelpBox("���� ��ȭ�� ������ �ڵ����� ����� ���� DialogueData ����(addressable data)\ncsv���� �����ߴٸ� �ڵ� ��� / �������� ��� ��� ���ص� ����", MessageType.Info);
        EditorGUILayout.Space();


        EditorGUILayout.PropertyField(dialoguesProp, new GUIContent("line list"), true);

        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif