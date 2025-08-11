using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameStartNameInput : MonoBehaviour
{
    [Header("UI 컴포넌트")]
    [SerializeField] private GameObject nameInputPanel;
    [SerializeField] private TMP_InputField lastNameInputField;
    [SerializeField] private TMP_InputField firstNameInputField;
    [SerializeField] private Button confirmButton;

    private const string InitialSetupKey = "InitialSetupDone";

    private Action onConfirmCallback;

    private void Start()
    {
        confirmButton.onClick.AddListener(OnConfirm);
        nameInputPanel.SetActive(false);
    }

    public void ShowNameInputPanel(Action onConfirm)
    {
        onConfirmCallback = onConfirm;

        nameInputPanel.SetActive(true);
        lastNameInputField.text = NameChangeManager.PlayerLastName;
        firstNameInputField.text = NameChangeManager.PlayerFirstName;
    }

    private void OnConfirm()
    {
        NameChangeManager.PlayerLastName = lastNameInputField.text;
        NameChangeManager.PlayerFirstName = firstNameInputField.text;

        PlayerPrefs.SetString("PlayerLastName", NameChangeManager.PlayerLastName);
        PlayerPrefs.SetString("PlayerFirstName", NameChangeManager.PlayerFirstName);

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.SetPlayerName(NameChangeManager.PlayerFirstName, NameChangeManager.PlayerLastName);
        }

        PlayerPrefs.SetInt(InitialSetupKey, 1);
        PlayerPrefs.Save();
        Debug.Log("이름 설정 완료");

        nameInputPanel.SetActive(false);

        onConfirmCallback?.Invoke();
    }
}