using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ContactUI : MonoBehaviour
{
    [Header("연락처 데이터")]
    [SerializeField] private ContactData contactData;

    [Header("UI 컴포넌트")]
    [SerializeField] private GameObject contactRoot;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Image profileImage;
    [SerializeField] private Slider affectionSlider;

    private bool isUnlocked = false;

    private void OnEnable()
    {
        FlagManager.OnFlagChanged += HandleFlagChange;
    }

    private void OnDisable()
    {
        FlagManager.OnFlagChanged -= HandleFlagChange;
    }

    private void Start()
    {
        if (contactData == null)
        {
            Debug.LogError($"오류: '{gameObject.name}'");
            gameObject.SetActive(false);
            return;
        }

        contactRoot.SetActive(false);
        if (affectionSlider != null)
        {
            affectionSlider.gameObject.SetActive(false);
        }

        CheckUnlockConditions();
    }

    private void Update()
    {
        if (isUnlocked && affectionSlider != null && affectionSlider.gameObject.activeSelf)
        {
            UpdateAffectionBar();
        }
    }

    private void HandleFlagChange(string changedFlagKey)
    {
        if (isUnlocked) return;

        CheckUnlockConditions();
    }

    private void CheckUnlockConditions()
    {
        if (isUnlocked || FlagManager.Instance == null) return;

        bool conditionsMet = contactData.requiredFlags.Any(flag => FlagManager.Instance.GetFlag(flag));

        if (conditionsMet)
        {
            isUnlocked = true;
            EnableContact();
        }
    }

    private void EnableContact()
    {
        Debug.Log($"'{contactData.contactName}' 연락처 저장");
        contactRoot.SetActive(true);
        nameText.text = contactData.contactName;
        messageText.text = contactData.contactMessage;
        profileImage.sprite = contactData.contactImage;

        if (affectionSlider != null && !string.IsNullOrEmpty(contactData.characterName))
        {
            affectionSlider.gameObject.SetActive(true);
            UpdateAffectionBar();
        }
    }

    private void UpdateAffectionBar()
    {
        if (AffectionManager.Instance != null && !string.IsNullOrEmpty(contactData.characterName))
        {
            float currentAffection = AffectionManager.Instance.GetAffection(contactData.characterName);
            affectionSlider.value = Mathf.Clamp01(currentAffection / 100f);
        }
    }
}