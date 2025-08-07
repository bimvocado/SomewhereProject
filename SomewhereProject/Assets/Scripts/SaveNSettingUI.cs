using UnityEngine;

public class SaveNSettingUI : MonoBehaviour
{
    public static SaveNSettingUI Instance { get; private set; }

    [SerializeField] private GameObject settingCanvas;
    [SerializeField] private GameObject saveCanvas;

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

    private void Start()
    {
        settingCanvas.SetActive(false);
        saveCanvas.SetActive(false);
    }

    public void ShowSettingUI()
    {
        settingCanvas.SetActive(true);
    }

    public void ShowSaveUI ()
    {
        saveCanvas.SetActive(true);
    } 
}
