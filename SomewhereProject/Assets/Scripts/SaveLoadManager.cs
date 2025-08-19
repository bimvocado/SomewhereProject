using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; private set; }
    private static GameData dataToLoad = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadGame(int slotIndex)
    {
        string savePath = GetPathForSlot(slotIndex);
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            dataToLoad = JsonUtility.FromJson<GameData>(json);
            SceneManager.LoadScene(dataToLoad.currentSceneName);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (dataToLoad != null)
        {
            ApplyGameData(dataToLoad);
            dataToLoad = null;
        }
    }

    private void ApplyGameData(GameData data)
    {
        Dictionary<string, int> affectionData = new Dictionary<string, int>();
        for (int i = 0; i < data.affectionKeys.Count; i++)
        {
            affectionData[data.affectionKeys[i]] = data.affectionValues[i];
        }
        AffectionManager.Instance.LoadAffections(affectionData);

        Dictionary<string, bool> flagData = new Dictionary<string, bool>();
        for (int i = 0; i < data.flagKeys.Count; i++)
        {
            flagData[data.flagKeys[i]] = data.flagValues[i];
        }
        FlagManager.Instance.LoadFlags(flagData);

        NameChangeManager.Instance.LoadName(data.playerFirstName, data.playerLastName);
        ReadLogManager.Instance.LoadReadLog(data.readDialogueLog);

        if (BackgroundManager.Instance != null && !string.IsNullOrEmpty(data.currentBackgroundName))
        {
            var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<Texture2D>(data.currentBackgroundName);
            handle.Completed += (op) =>
            {
                if (op.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    var texture = op.Result;
                    Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    BackgroundManager.Instance.ChangeBackground(newSprite);
                }
            };
        }

        DialogueManager.Instance.LoadDialogueState(data.currentDialogueAssetKey, data.currentDialogueIndex);
        Debug.Log("적용완료");
    }

    private string GetPathForSlot(int slotIndex)
    {
        return Path.Combine(Application.persistentDataPath, $"savedata_{slotIndex}.json");
    }

    public GameData GetSaveDataInfo(int slotIndex)
    {
        string savePath = GetPathForSlot(slotIndex);
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<GameData>(json);
        }
        return null;
    }

    public void SaveGame(int slotIndex)
    {
        GameData gameData = new GameData();

        Dictionary<string, int> affectionData = AffectionManager.Instance.GetAffections();
        gameData.affectionKeys = affectionData.Keys.ToList();
        gameData.affectionValues = affectionData.Values.ToList();

        Dictionary<string, bool> flagData = FlagManager.Instance.GetFlags();
        gameData.flagKeys = flagData.Keys.ToList();
        gameData.flagValues = flagData.Values.ToList();

        DialogueManager.Instance.GetCurrentDialogueState(out gameData.currentDialogueAssetKey, out gameData.currentDialogueIndex);
        gameData.playerFirstName = NameChangeManager.PlayerFirstName;
        gameData.playerLastName = NameChangeManager.PlayerLastName;
        gameData.currentSceneName = SceneManager.GetActiveScene().name;
        gameData.readDialogueLog = ReadLogManager.Instance.GetReadLog();
        if (SceneLoader.CurrentEpisodeData != null) gameData.episodeName = SceneLoader.CurrentEpisodeData.EpisodeName;
        if (BackgroundManager.Instance != null) gameData.currentBackgroundName = BackgroundManager.Instance.GetCurrentBackgroundName();
        gameData.saveTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

        string json = JsonUtility.ToJson(gameData, true);
        string savePath = GetPathForSlot(slotIndex);
        File.WriteAllText(savePath, json);
        Debug.Log($"게임 데이터 저장 완료 (플래그 {gameData.flagKeys.Count}개, 슬롯 {slotIndex}): {savePath}");
    }

    public void DeleteSaveData(int slotIndex)
    {
        string savePath = GetPathForSlot(slotIndex);
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
    }
}