using UnityEngine;

public class StartGameButton : MonoBehaviour
{
    [Header("로드할 씬")]
    public string sceneNameToLoad;
    public EpisodeData episodeData;

    private const string InitialSetupKey = "InitialSetupDone";
    private GameStartNameInput nameInputController;

    private void Start()
    {
        nameInputController = FindFirstObjectByType<GameStartNameInput>();
    }

    public void OnClickStart()
    {
        if (PlayerPrefs.GetInt(InitialSetupKey, 0) == 1)
        {
            Debug.Log("이름 등록 완료됨.");
            LoadGameScene();
        }
        else
        {
            Debug.Log("이름 설정 필요");
            if (nameInputController != null)
            {

                nameInputController.ShowNameInputPanel(LoadGameScene);
            }
        }
    }

    private void LoadGameScene()
    {
        if (SceneLoader.Instance != null && !string.IsNullOrEmpty(sceneNameToLoad))
        {
            SceneLoader.Instance.LoadScene(sceneNameToLoad, episodeData);
        }
    }
}