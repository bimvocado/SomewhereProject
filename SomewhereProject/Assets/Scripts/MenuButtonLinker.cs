using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Coroutine�� ���� �߰�

// �� ��ũ��Ʈ�� �ɼ�/���� ��ư�� ���Դϴ�.
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
            Debug.LogError($"{gameObject.name}: Button ������Ʈ�� ã�� �� �����ϴ�!");
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
                Debug.LogWarning($"{gameObject.name}: �� �� ���� ����: {actionToPerform}");
                break;
        }
    }
}