using UnityEngine;

public class SceneEventLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private EpisodeData episodeData;

    public void LoadSceneEvent()
    {
        if (SceneLoader.Instance != null && !string.IsNullOrEmpty(sceneName))
        {
            SceneLoader.Instance.LoadScene(sceneName, episodeData);
        }
    }

    public void LoadSceneName(string sceneName)
    {
        if (SceneLoader.Instance != null && !string.IsNullOrEmpty(sceneName))
        {
            SceneLoader.Instance.LoadScene(sceneName);
        }
    }
}
