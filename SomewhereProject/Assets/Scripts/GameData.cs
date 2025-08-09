using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int playerCoin;
    public string currentSceneName;
    public Dictionary<string, int> affectionData;
    public Dictionary<string, bool> flagData;
    public string currentDialogueAssetKey;
    public int currentDialogueIndex;
    public HashSet<string> readDialogueLog;
    public string playerFirstName;
    public string playerLastName;

    public GameData()
    {
        playerCoin = 0;
        affectionData = new Dictionary<string, int>();
        flagData = new Dictionary<string, bool>();
        currentDialogueAssetKey = null;
        currentDialogueIndex = 0;
        readDialogueLog = new HashSet<string>();
        playerFirstName = "여주";
        playerLastName = "김";
    }
}