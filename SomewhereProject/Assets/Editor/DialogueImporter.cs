#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.AddressableAssets;


public class DialogueImporter : EditorWindow
{
    private TextAsset csvFile;
    private string savePath = "Assets/DialogueData/";
    private string fileName = "ImportedDialogue";
    private bool createSeparateFiles = false;

    private class TempChoiceData
    {
        public Choice choiceInstance;
        public string tempNextDialogueName;
    }

    private class TempDialogueDataHolder
    {
        public DialogueData dialogueDataAsset;
        public string tempNextOnCompletionName;
        public List<TempChoiceData> tempChoices = new List<TempChoiceData>();
    }

    [System.Serializable]
    private class DialogueRow
    {
        public string speaker, line, conditionType, conditionKey, hasChoices, eventName, choicesData, groupId, nextOnCompletion, conditionTargetCharacter, conditionValue;
    }

    [MenuItem("Dialogue/Advanced CSV Importer")]
    public static void ShowWindow()
    {
        GetWindow<DialogueImporter>("CSV Importer");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("CSV to DialogueData Importer", EditorStyles.boldLabel);
        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV     ", csvFile, typeof(TextAsset), false);
        savePath = EditorGUILayout.TextField("        ", savePath);
        fileName = EditorGUILayout.TextField("      ̸  (groupId        )", fileName);
        createSeparateFiles = EditorGUILayout.Toggle(" ׷캰         и ", createSeparateFiles);
        EditorGUILayout.Space();
        GUI.enabled = csvFile != null;
        if (GUILayout.Button("         (Import)"))
        {
            ImportCSV();
        }
        GUI.enabled = true;
    }

    private void ImportCSV()
    {
        if (csvFile == null) return;
        List<DialogueRow> rows = ParseCSVData(csvFile.text);
        if (rows.Count == 0) return;
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        List<TempDialogueDataHolder> tempHolders = new List<TempDialogueDataHolder>();
        if (createSeparateFiles)
        {
            var groups = rows.GroupBy(r => r.groupId);
            foreach (var group in groups)
            {
                string groupName = string.IsNullOrEmpty(group.Key) ? "default" : group.Key;
                DialogueData data = ScriptableObject.CreateInstance<DialogueData>();
                TempDialogueDataHolder holder = PopulateDialogueData(data, group.ToList());
                holder.tempNextOnCompletionName = group.First().nextOnCompletion;
                AssetDatabase.CreateAsset(data, Path.Combine(savePath, $"{groupName}.asset"));
                tempHolders.Add(holder);
            }
        }
        else
        {
            DialogueData data = ScriptableObject.CreateInstance<DialogueData>();
            TempDialogueDataHolder holder = PopulateDialogueData(data, rows);
            holder.tempNextOnCompletionName = rows.First().nextOnCompletion;
            AssetDatabase.CreateAsset(data, Path.Combine(savePath, $"{fileName}.asset"));
            tempHolders.Add(holder);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        foreach (var holder in tempHolders)
        {
            if (!string.IsNullOrEmpty(holder.tempNextOnCompletionName))
            {
                DialogueData resolvedNextData = FindAndLoadDialogueData(holder.tempNextOnCompletionName);
                if (resolvedNextData != null)
                {
                    string path = AssetDatabase.GetAssetPath(resolvedNextData);
                    string guid = AssetDatabase.AssetPathToGUID(path);
                    holder.dialogueDataAsset.nextDialogueOnCompletion = new AssetReferenceT<DialogueData>(guid);

                    EditorUtility.SetDirty(holder.dialogueDataAsset);
                }
            }

            foreach (var tempChoice in holder.tempChoices)
            {
                if (!string.IsNullOrEmpty(tempChoice.tempNextDialogueName))
                {
                    DialogueData resolvedNextDialogue = FindAndLoadDialogueData(tempChoice.tempNextDialogueName);
                    if (resolvedNextDialogue != null)
                    {
                        string path = AssetDatabase.GetAssetPath(resolvedNextDialogue);
                        string guid = AssetDatabase.AssetPathToGUID(path);
                        tempChoice.choiceInstance.nextDialogue = new AssetReferenceT<DialogueData>(guid);

                        EditorUtility.SetDirty(holder.dialogueDataAsset);
                    }
                }
            }
        }
        AssetDatabase.SaveAssets();
        Debug.Log($"Imported {rows.Count} dialogue entries and resolved references.");
    }

    private List<DialogueRow> ParseCSVData(string csvText)
    {
        List<DialogueRow> rows = new List<DialogueRow>();
        string[] lines = csvText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length <= 1) return rows;
        string[] headers = ParseCSVLine(lines[0]);
        Dictionary<string, int> columnMap = new Dictionary<string, int>();
        for (int i = 0; i < headers.Length; i++)
        {
            columnMap[headers[i].ToLower().Trim()] = i;
        }
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = ParseCSVLine(lines[i]);
            if (values.Length < 1 || string.IsNullOrEmpty(values[0])) continue;
            DialogueRow row = new DialogueRow
            {
                groupId = GetValue(values, columnMap, "groupid", "default"),
                speaker = GetValue(values, columnMap, "speaker"),
                line = GetValue(values, columnMap, "line"),
                hasChoices = GetValue(values, columnMap, "haschoices", "false"),
                choicesData = GetValue(values, columnMap, "choices"),
                conditionType = GetValue(values, columnMap, "conditiontype", "None"),
                conditionKey = GetValue(values, columnMap, "conditionkey"),
                nextOnCompletion = GetValue(values, columnMap, "nextoncompletion", ""),
                conditionTargetCharacter = GetValue(values, columnMap, "conditiontargetcharacter"),
                conditionValue = GetValue(values, columnMap, "conditionvalue", "0"),
                eventName = GetValue(values, columnMap, "eventname")
            };
            rows.Add(row);
        }
        return rows;
    }

    private string[] ParseCSVLine(string line)
    {
        List<string> result = new List<string>();
        bool inQuotes = false;
        string currentField = "";
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (c == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    currentField += '"';
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(currentField);
                currentField = "";
            }
            else
            {
                currentField += c;
            }
        }
        result.Add(currentField);
        return result.ToArray();
    }

    private string GetValue(string[] values, Dictionary<string, int> columnMap, string columnName, string defaultValue = "")
    {
        columnName = columnName.ToLower().Trim();
        if (columnMap.ContainsKey(columnName) && columnMap[columnName] < values.Length)
        {
            string value = values[columnMap[columnName]];
            value = value.Trim();
            if (value.Length >= 2 && value.StartsWith("\"") && value.EndsWith("\""))
            {
                value = value.Substring(1, value.Length - 2);
            }
            value = value.Replace("\"\"", "\"");
            return value.Replace("\\n", "\n");
        }
        return defaultValue;
    }

    private TempDialogueDataHolder PopulateDialogueData(DialogueData data, List<DialogueRow> rows)
    {
        TempDialogueDataHolder holder = new TempDialogueDataHolder { dialogueDataAsset = data };
        List<Dialogue> dialogues = new List<Dialogue>();
        foreach (var row in rows)
        {
            Dialogue dialogue = new Dialogue
            {
                speaker = row.speaker,
                line = row.line,
                eventName = row.eventName
            };
            if (Enum.TryParse<ConditionType>(row.conditionType, true, out var conditionType))
            {
                dialogue.conditionType = conditionType;
                dialogue.conditionKey = row.conditionKey;
                dialogue.conditionTargetCharacter = row.conditionTargetCharacter;
                if (int.TryParse(row.conditionValue, out int value))
                {
                    dialogue.conditionValue = value;
                }
            }
            if (bool.TryParse(row.hasChoices, out bool hasChoices) && hasChoices && !string.IsNullOrEmpty(row.choicesData))
            {
                dialogue.hasChoices = true;
                dialogue.choices = ParseChoicesAndCollectTempData(row.choicesData, holder.tempChoices);
            }
            dialogues.Add(dialogue);
        }
        data.dialogues = dialogues.ToArray();
        return holder;
    }

    private Choice[] ParseChoicesAndCollectTempData(string choicesData, List<TempChoiceData> tempChoicesList)
    {
        List<Choice> choices = new List<Choice>();
        string[] choiceEntries = choicesData.Split(';');

        foreach (string entry in choiceEntries)
        {
            if (string.IsNullOrWhiteSpace(entry)) continue;

            string[] parts = entry.Split('|');
            if (parts.Length < 1) continue;

            Choice choice = new Choice { text = parts[0].Trim() };
            string tempNextDialogue = (parts.Length > 1) ? parts[1].Trim() : "";

            if (parts.Length > 2 && int.TryParse(parts[2].Trim(), out int affection))
            {
                choice.affectionChange = affection;
            }
            if (parts.Length > 3)
            {
                choice.targetCharacterForAffection = parts[3].Trim();
            }

            if (parts.Length > 4 && bool.TryParse(parts[4].Trim(), out bool resetsAffection))
            {
                choice.resetsAffection = resetsAffection;
            }

            if (parts.Length > 5 && int.TryParse(parts[4].Trim(), out int coin))
            {
                choice.requiredCoin = coin;
            }

            if (parts.Length > 6 && !string.IsNullOrWhiteSpace(parts[6]))
            {
                choice.flagToSet = parts[6].Trim();
                choice.flagValue = true;
            }

            if (parts.Length > 7 && Enum.TryParse<ConditionType>(parts[7].Trim(), true, out var conditionType))
            {
                choice.conditionType = conditionType;
            }

            if (parts.Length > 8)
            {
                choice.conditionKey = parts[8].Trim();
            }
            if (parts.Length > 9)
            {
                choice.conditionTargetCharacter = parts[9].Trim();
            }
            if (parts.Length > 10 && int.TryParse(parts[10].Trim(), out int conditionValue))
            {
                choice.conditionValue = conditionValue;
            }

            tempChoicesList.Add(new TempChoiceData
            {
                choiceInstance = choice,
                tempNextDialogueName = tempNextDialogue
            });
            choices.Add(choice);
        }
        return choices.ToArray();
    }

    private DialogueData FindAndLoadDialogueData(string dialogueName)
    {
        if (string.IsNullOrEmpty(dialogueName)) return null;
        string[] guids = AssetDatabase.FindAssets($"t:DialogueData {dialogueName}");

        if (guids.Length > 0)
        {
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (Path.GetFileNameWithoutExtension(path).Equals(dialogueName, StringComparison.OrdinalIgnoreCase))
                {
                    return AssetDatabase.LoadAssetAtPath<DialogueData>(path);
                }
            }
            return AssetDatabase.LoadAssetAtPath<DialogueData>(AssetDatabase.GUIDToAssetPath(guids[0]));
        }
        return null;
    }
}
#endif