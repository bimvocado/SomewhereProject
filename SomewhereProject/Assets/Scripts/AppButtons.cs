using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AppButton : MonoBehaviour
{
    [Header("�� ��ư�� ������ �� ���� ��")]
    [SerializeField] private GameObject appUI;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OpenApp);
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OpenApp);
        }
    }

    private void OpenApp()
    {
        if (appUI == null)
        {
            Debug.LogError($"{gameObject.name}�� ����� �� UI ����");
            return;
        }

        BarUIManager.Instance.ShowApp(appUI);
    }
}