using System;
using System.IO;
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
        else
        {
            Debug.Log($"슬롯 {slotIndex}에 저장된 파일이 없습니다.");
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
        AffectionManager.Instance.LoadAffections(data.affectionData);
        FlagManager.Instance.LoadFlags(data.flagData);
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
                else
                {
                    Debug.LogError($"어드레서블 로딩 실패! 키: {data.currentBackgroundName}");
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
        else
        {
            return null;
        }
    }

    public void SaveGame(int slotIndex)
    {
        if (BackgroundManager.Instance == null)
        {
            return;
        }

        string backgroundName = BackgroundManager.Instance.GetCurrentBackgroundName();

        GameData gameData = new GameData
        {
            affectionData = AffectionManager.Instance.GetAffections(),
            flagData = FlagManager.Instance.GetFlags()
        };
        DialogueManager.Instance.GetCurrentDialogueState(out gameData.currentDialogueAssetKey, out gameData.currentDialogueIndex);
        gameData.playerFirstName = NameChangeManager.PlayerFirstName;
        gameData.playerLastName = NameChangeManager.PlayerLastName;
        gameData.currentSceneName = SceneManager.GetActiveScene().name;
        gameData.readDialogueLog = ReadLogManager.Instance.GetReadLog();

        if (SceneLoader.CurrentEpisodeData != null) gameData.episodeName = SceneLoader.CurrentEpisodeData.EpisodeName;

        gameData.currentBackgroundName = backgroundName;

        gameData.saveTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

        string json = JsonUtility.ToJson(gameData, true);
        string savePath = GetPathForSlot(slotIndex);
        File.WriteAllText(savePath, json);
        Debug.Log($"게임 데이터 저장 완료 (슬롯 {slotIndex}): {savePath}");
    }

    public void DeleteSaveData(int slotIndex)
    {
        string savePath = GetPathForSlot(slotIndex);

        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log($"<color=red>슬롯 {slotIndex + 1}의 저장 파일 삭제 완료:</color> {savePath}");
        }
        else
        {
            Debug.LogWarning($"삭제할 파일 없음: 슬롯 {slotIndex + 1}에 저장된 데이터가 없습니다.");
        }
    }

}