using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MinigameSceneSwitcher : MonoBehaviour
{
    [Header("다음으로 불러올 씬")]
    public string sceneToLoad;

    [Header("현재 씬 (닫을 씬)")]
    public string sceneToUnload;

    public void Switch()
    {
        if (MinigameManager.Instance != null) StartCoroutine(SwitchRoutine());
        else Debug.LogError("MinigameManager를 찾을 수 없습니다!");
    }

    private IEnumerator SwitchRoutine()
    {
        MinigameManager.Instance.UpdateMinigameScenes(sceneToUnload, sceneToLoad);
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            yield return SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));
        }
        if (!string.IsNullOrEmpty(sceneToUnload))
        {
            yield return null;
            yield return SceneManager.UnloadSceneAsync(sceneToUnload);
        }
    }
}