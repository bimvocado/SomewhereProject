using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    public static EpisodeData CurrentEpisodeData { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName, EpisodeData episodeData = null)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("불러올 씬 이름이 비어 있음");
            return;
        }
        StartCoroutine(LoadSceneAsync(sceneName, episodeData));
    }

    private IEnumerator LoadSceneAsync(string sceneName, EpisodeData episodeData)
    {
        CurrentEpisodeData = episodeData;
        LoadingManager.Instance.ShowLoadingScreen(episodeData);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        LoadingManager.Instance.HideLoadingScreen();
    }
}