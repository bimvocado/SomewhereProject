using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ConfirmationPopup : MonoBehaviour
{
    public static ConfirmationPopup Instance { get; private set; }

    [Header("UI 컴포넌트 연결")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        popupPanel.SetActive(false);
    }

    public void Show(string message, string cost, Action onConfirm)
    {
        messageText.text = message;

        if (costText != null)
        {
            if (!string.IsNullOrEmpty(cost))
            {
                costText.gameObject.SetActive(true);
                costText.text = cost;
            }
            else
            {
                costText.gameObject.SetActive(false);
            }
        }

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() => {
            onConfirm?.Invoke();
            Hide();
        });
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(Hide);

        popupPanel.SetActive(true);
    }

    private void Hide()
    {
        popupPanel.SetActive(false);
    }

    public void ClosePopup()
    {
        Hide();
    }
}