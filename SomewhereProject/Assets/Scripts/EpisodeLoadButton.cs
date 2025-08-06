using UnityEngine;

public class EpisodeLoadButton : MonoBehaviour
{
    public string sceneNameToLoad;
    public EpisodeData episodeData;

    public void LoadEpisodeScene()
    {
        SceneLoader.Instance.LoadScene(sceneNameToLoad, episodeData);
    }
}
