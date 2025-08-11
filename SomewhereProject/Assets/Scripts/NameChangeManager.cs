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
        if (Instance == null)
        {
            Instance = this;
            LoadNameFromPlayerPrefs();
        }
        else Destroy(gameObject);
    }

    private void LoadNameFromPlayerPrefs()
    {
        PlayerLastName = PlayerPrefs.GetString("PlayerLastName", "김");
        PlayerFirstName = PlayerPrefs.GetString("PlayerFirstName", "여주");

        Debug.Log($"PlayerPrefs에서 이름 불러오기 완료: {PlayerLastName}{PlayerFirstName}");
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
        PlayerPrefs.Save();
        CloseNameChangePanel();
    }

    public void LoadName(string firstName, string lastName)
    {
        PlayerFirstName = firstName;
        PlayerLastName = lastName;
        DialogueManager.Instance.SetPlayerName(PlayerFirstName, PlayerLastName);
    }
}