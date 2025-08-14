using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public static GameLoopManager Instance { get; private set; }

    [Header("게임 루프 설정")]
    public string firstEpisodeSceneName = "episode1";

    private const string GameLoopEventName = "GAME_LOOP";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.StartListening(GameLoopEventName, HandleGameLoop);
        }
    }

    private void HandleGameLoop()
    {
        Debug.Log("루프 준비");

        if (AffectionManager.Instance != null)
        {
            AffectionManager.Instance.ResetAllAffections();
        }

        if (SceneLoader.Instance != null && !string.IsNullOrEmpty(firstEpisodeSceneName))
        {
            SceneLoader.Instance.LoadScene(firstEpisodeSceneName);
        }
    }

    private void OnDestroy()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.StopListening(GameLoopEventName, HandleGameLoop);
        }
    }
}