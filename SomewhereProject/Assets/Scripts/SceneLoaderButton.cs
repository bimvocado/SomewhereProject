using UnityEngine;

public class SceneLoaderButton : MonoBehaviour
{
    public string loadSceneName;

    public void LoadScene()
    {
        SceneLoader.Instance.LoadScene(loadSceneName, null);
    }
}
