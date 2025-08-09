using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameInputManager : MonoBehaviour
{
    public static NameInputManager Instance { get; private set; }

    [SerializeField] private TMP_InputField lastNameInputField;
    [SerializeField] private TMP_InputField firstNameInputField;
    [SerializeField] private Button confirmButton;
    [SerializeField] private GameObject nameInputPanel;

    public static string PlayerFirstName { get; private set; }
    public static string PlayerLastName { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        lastNameInputField.text = "김";
        firstNameInputField.text = "여주";
        confirmButton.onClick.AddListener(OnConfirmButtonClick);
    }

    private void OnConfirmButtonClick()
    {
        string inputLastName = lastNameInputField.text;
        string inputFirstName = firstNameInputField.text;

        PlayerFirstName = inputFirstName;
        PlayerLastName = inputLastName;
        nameInputPanel.SetActive(false);

        DialogueManager.Instance.SetPlayerName(PlayerFirstName, PlayerLastName);
    }

    public void ShowNameInputPanel(bool show)
    {
        nameInputPanel.SetActive(show);
    }

    public void OpenNameChangePanel()
    {
        nameInputPanel.SetActive(true);

        lastNameInputField.text = PlayerLastName;
        firstNameInputField.text = PlayerFirstName;
    }


    public void LoadName(string firstName, string lastName)
    {
        PlayerFirstName = firstName;
        PlayerLastName = lastName;
        DialogueManager.Instance.SetPlayerName(PlayerFirstName, PlayerLastName);
    }
}
