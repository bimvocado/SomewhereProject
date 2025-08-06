using UnityEngine;
using System.Collections.Generic;

public class ReadLogManager : MonoBehaviour
{
    public static ReadLogManager Instance { get; private set; }

    private HashSet<string> readDialogueLog = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MarkAsRead(string dialogueAssetKey, int dialogueIndex)
    {
        readDialogueLog.Add(GenerateKey(dialogueAssetKey, dialogueIndex));
    }

    public bool IsRead(string dialogueAssetKey, int dialogueIndex)
    {
        return readDialogueLog.Contains(GenerateKey(dialogueAssetKey, dialogueIndex));
    }

    public HashSet<string> GetReadLog()
    {
        return readDialogueLog;
    }

    public void LoadReadLog(HashSet<string> data)
    {
        if (data != null)
        {
            readDialogueLog = data;
        }
    }

    private string GenerateKey(string dialogueAssetKey, int dialogueIndex)
    {
        return $"{dialogueAssetKey}_{dialogueIndex}";
    }
}