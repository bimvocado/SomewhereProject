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
        // �ν��Ͻ��� �ı��Ǿ��ų�, ������ ���� ���ٸ� ���� ����
        if (loadingScreenInstance == null)
        {
            // �������� ����ִ��� �ٽ� �ѹ� Ȯ��
            if (loadingScreenPrefab == null)
            {
                Debug.LogError("LoadingScreenPrefab�� LoadingManager�� ������� �ʾҽ��ϴ�!");
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