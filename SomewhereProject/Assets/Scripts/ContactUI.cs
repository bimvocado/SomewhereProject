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

    private void Start()
    {
        if (contactData == null)
        {
            gameObject.SetActive(false);
            return;
        }

        contactRoot.SetActive(false);
        if (affectionSlider != null)
        {
            affectionSlider.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isUnlocked)
        {
            CheckUnlockConditions();
        }

        if (isUnlocked && affectionSlider != null && affectionSlider.gameObject.activeSelf)
        {
            UpdateAffectionBar();
        }
    }

    private void CheckUnlockConditions()
    {
        if (FlagManager.Instance == null || contactData.requiredFlags == null || contactData.requiredFlags.Count == 0)
        {
            return;
        }

        bool conditionsMet = contactData.requiredFlags.Any(flag => FlagManager.Instance.GetFlag(flag));

        if (conditionsMet)
        {
            isUnlocked = true;
            EnableContact();
        }
    }

    private void EnableContact()
    {
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