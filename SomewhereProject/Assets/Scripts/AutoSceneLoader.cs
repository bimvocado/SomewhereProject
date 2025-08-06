using System.Collections;
using UnityEngine;

public class AutoSceneLoader : MonoBehaviour
{
    public string nextSceneName;
    public EpisodeData episodeData;
    public float delaySeconds = 2.0f;

    private void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(delaySeconds);

        SceneLoader.Instance.LoadScene(nextSceneName, episodeData);
    }
}
