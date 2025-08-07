using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Coroutine을 위해 추가

// 이 스크립트를 옵션/저장 버튼에 붙입니다.
public class MenuButtonLinker : MonoBehaviour
{
    public enum ButtonAction { ShowSettings, ShowSave }

    public ButtonAction actionToPerform;

    private Button thisButton;

    private void Awake()
    {
        thisButton = GetComponent<Button>();
        if (thisButton == null)
        {
            Debug.LogError($"{gameObject.name}: Button 컴포넌트를 찾을 수 없습니다!");
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        StartCoroutine(AddListenerWhenReady());
    }

    private IEnumerator AddListenerWhenReady()
    {
        while (SaveNSettingUI.Instance == null)
        {
            yield return null;
        }

        thisButton.onClick.RemoveAllListeners();

        switch (actionToPerform)
        {
            case ButtonAction.ShowSettings:
                thisButton.onClick.AddListener(() => SaveNSettingUI.Instance.ShowSettingUI());
                break;
            case ButtonAction.ShowSave:
                thisButton.onClick.AddListener(() => SaveNSettingUI.Instance.ShowSaveUI());
                break;
            default:
                Debug.LogWarning($"{gameObject.name}: 알 수 없는 동작: {actionToPerform}");
                break;
        }
    }
}