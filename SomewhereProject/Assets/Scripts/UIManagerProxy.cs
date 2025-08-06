using UnityEngine;

public class UIManagerProxy : MonoBehaviour
{
    public void ShowSettingUI()
    {
        if (SaveNSettingUI.Instance != null)
        {
            SaveNSettingUI.Instance.ShowSettingUI();
        }
    }

    public void ShowSaveUI()
    {
        if (SaveNSettingUI.Instance != null)
        {
            SaveNSettingUI.Instance.ShowSaveUI();
        }
    }
}
