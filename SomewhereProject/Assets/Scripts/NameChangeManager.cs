using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameChangeManager : MonoBehaviour
{
    public static NameChangeManager Instance { get; private set; }

    [Header("change name input UI")]
    [SerializeField] private GameObject nameChangePanel;
    [SerializeField] private TMP_InputField lastNameInputField_Settings;
    [SerializeField] private TMP_InputField firstNameInputField_Settings;
    [SerializeField] private Button confirmButton_Settings;

    public static string PlayerFirstName { get; set; } = "여주";
    public static string PlayerLastName { get; set; } = "김";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        confirmButton_Settings.onClick.AddListener(OnConfirmButtonClick);

        if (nameChangePanel != null)
        {
            nameChangePanel.SetActive(false);
        }
    }

    public void OpenNameChangePanel()
    {
        if (nameChangePanel == null) return;

        nameChangePanel.SetActive(true);
        lastNameInputField_Settings.text = PlayerLastName;
        firstNameInputField_Settings.text = PlayerFirstName;
    }

    public void CloseNameChangePanel()
    {
        if (nameChangePanel == null) return;
        nameChangePanel.SetActive(false);
    }

    public void OnConfirmButtonClick()
    {
        PlayerLastName = lastNameInputField_Settings.text;
        PlayerFirstName = firstNameInputField_Settings.text;

        DialogueManager.Instance.SetPlayerName(PlayerFirstName, PlayerLastName);

        CloseNameChangePanel();
    }

    public void LoadName(string firstName, string lastName)
    {
        PlayerFirstName = firstName;
        PlayerLastName = lastName;
        DialogueManager.Instance.SetPlayerName(PlayerFirstName, PlayerLastName);
    }
}