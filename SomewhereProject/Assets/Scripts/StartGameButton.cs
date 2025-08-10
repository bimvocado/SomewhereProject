using UnityEngine;

public class StartGameButton : MonoBehaviour
{
    [Header("�ε��� ��")]
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
            Debug.Log("�̸� ��� �Ϸ��.");
            LoadGameScene();
        }
        else
        {
            Debug.Log("�̸� ���� �ʿ�");
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