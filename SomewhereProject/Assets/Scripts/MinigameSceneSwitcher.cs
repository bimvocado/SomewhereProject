using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MinigameSceneSwitcher : MonoBehaviour
{
    [Header("�������� �ҷ��� ��")]
    public string sceneToLoad;

    [Header("���� �� (���� ��)")]
    public string sceneToUnload;

    public void Switch()
    {
        if (MinigameManager.Instance != null) StartCoroutine(SwitchRoutine());
        else Debug.LogError("MinigameManager�� ã�� �� �����ϴ�!");
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