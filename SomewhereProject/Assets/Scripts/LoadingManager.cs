using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance { get; private set; }

    [SerializeField]
    private GameObject loadingScreenPrefab;
    private GameObject loadingScreenInstance;

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

    public void ShowLoadingScreen(EpisodeData episodeData)
    {
        // 인스턴스가 파괴되었거나, 생성된 적이 없다면 새로 생성
        if (loadingScreenInstance == null)
        {
            // 프리팹이 비어있는지 다시 한번 확인
            if (loadingScreenPrefab == null)
            {
                Debug.LogError("LoadingScreenPrefab이 LoadingManager에 연결되지 않았습니다!");
                return;
            }
            loadingScreenInstance = Instantiate(loadingScreenPrefab);
            DontDestroyOnLoad(loadingScreenInstance);
        }

        loadingScreenInstance.SetActive(true);

        LoadingUI ui = loadingScreenInstance.GetComponent<LoadingUI>();
        if (ui != null)
        {
            ui.SetEpisodeInfo(episodeData);
        }
    }

    public void HideLoadingScreen()
    {
        if (loadingScreenInstance != null)
        {
            loadingScreenInstance.SetActive(false);
        }
    }
}