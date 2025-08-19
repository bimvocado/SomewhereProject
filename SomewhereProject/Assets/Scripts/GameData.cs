using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int playerCoin;
    public string currentSceneName;
    public List<string> affectionKeys;
    public List<int> affectionValues;
    public List<string> flagKeys;
    public List<bool> flagValues;
    public string currentDialogueAssetKey;
    public int currentDialogueIndex;
    public HashSet<string> readDialogueLog;
    public string playerFirstName;
    public string playerLastName;
    public string saveTimestamp;
    public string episodeName;
    public string currentBackgroundName;
    public GameData()
    {
        affectionKeys = new List<string>();
        affectionValues = new List<int>();
        flagKeys = new List<string>();
        flagValues = new List<bool>();
        currentDialogueAssetKey = null;
        currentDialogueIndex = 0;
        readDialogueLog = new HashSet<string>();
        playerFirstName = "여주";
        playerLastName = "김";
    }
}