using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; }

    private List<CharacterAnimationController> activeCharacters = new List<CharacterAnimationController>();
    private BackgroundController activeBackground;

    private List<GameObject> deactivatedObjects = new List<GameObject>();
    private List<string> currentMinigameScenes = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterCharacter(CharacterAnimationController character)
    {
        if (!activeCharacters.Contains(character))
        {
            activeCharacters.Add(character);
        }
    }

    public void UnregisterCharacter(CharacterAnimationController character)
    {
        if (activeCharacters.Contains(character))
        {
            activeCharacters.Remove(character);
        }
    }

    public void RegisterBackground(BackgroundController background)
    {
        activeBackground = background;
    }

    public void UnregisterBackground(BackgroundController background)
    {
        if (activeBackground == background)
        {
            activeBackground = null;
        }
    }

    public void StartMinigame(List<string> sceneNames)
    {
        currentMinigameScenes = new List<string>(sceneNames);
        StartCoroutine(LoadMinigameScenes(sceneNames));
    }

    private IEnumerator LoadMinigameScenes(List<string> sceneNames)
    {
        deactivatedObjects.Clear();

        foreach (CharacterAnimationController character in activeCharacters)
        {
            if (character != null && character.gameObject.activeSelf)
            {
                deactivatedObjects.Add(character.gameObject);
                character.gameObject.SetActive(false);
            }
        }
        if (activeBackground != null && activeBackground.gameObject.activeSelf)
        {
            deactivatedObjects.Add(activeBackground.gameObject);
            activeBackground.gameObject.SetActive(false);
        }
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.SetDialogueUIVisibility(false);
        }

        if (BarUIManager.Instance != null)
        {
            BarUIManager.Instance.phone.SetActive(false);
        }

        for (int i = 0; i < sceneNames.Count; i++)
        {
            yield return SceneManager.LoadSceneAsync(sceneNames[i], LoadSceneMode.Additive);
        }

        if (sceneNames.Count > 0)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneNames[0]));
        }
    }

    public void EndMinigame()
    {
        StartCoroutine(UnloadMinigameScenes());
    }

    private IEnumerator UnloadMinigameScenes()
    {
        foreach (string sceneName in currentMinigameScenes)
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                yield return SceneManager.UnloadSceneAsync(sceneName);
            }
        }
        currentMinigameScenes.Clear();

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.SetDialogueUIVisibility(true);
        }

        if (BarUIManager.Instance != null)
        {
            BarUIManager.Instance.phone.SetActive(true);
        }
        foreach (GameObject obj in deactivatedObjects)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        deactivatedObjects.Clear();
    }
}