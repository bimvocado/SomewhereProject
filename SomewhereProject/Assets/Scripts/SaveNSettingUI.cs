using UnityEngine;

public class SaveNSettingUI : MonoBehaviour
{
    public static SaveNSettingUI Instance { get; private set; }

    [SerializeField] private GameObject settingUI;
    [SerializeField] private GameObject saveUI;


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

    public bool IsUIShowing()
    {
        return (settingUI != null && settingUI.activeSelf) || (saveUI != null && saveUI.activeSelf);
    }

    public void ShowSettingUI()
    {
        settingUI.SetActive(true);
    }

    public void ShowSaveUI()
    {
        saveUI.SetActive(true);
    }

    public void CloseSettingUI()
    {
        settingUI.SetActive(false);
    }

    public void CloseSaveUI()
    {
        saveUI.SetActive(false);
    }
}
