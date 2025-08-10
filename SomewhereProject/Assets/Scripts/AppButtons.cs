using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AppButton : MonoBehaviour
{
    [Header("이 버튼을 눌렀을 때 열릴 앱")]
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
            Debug.LogError($"{gameObject.name}에 연결된 앱 UI 없음");
            return;
        }

        BarUIManager.Instance.ShowApp(appUI);
    }
}