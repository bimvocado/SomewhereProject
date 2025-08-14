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


    private void Start()
    {
        if (contactData == null)
        {
            return;
        }
        if (contactRoot == null)
        {
            return;
        }
        contactRoot.SetActive(false);
    }

    public void UpdateContactState()
    {
        if (contactData == null || FlagManager.Instance == null)
        {
            return;
        }

        bool conditionsMet = contactData.requiredFlags.Any(flag => FlagManager.Instance.GetFlag(flag));

        string flagsChecked = string.Join(", ", contactData.requiredFlags.Select(f => $"'{f}' = {FlagManager.Instance.GetFlag(f)}"));

        if (conditionsMet)
        {
            EnableContact();
        }
        else
        {
            DisableContact();
        }
    }

    private void EnableContact()
    {
        if (!contactRoot.activeSelf)
        {
            Debug.Log($"<color=green>ContactUI ({contactData.contactName}):</color> 활성화합니다.");
        }

        contactRoot.SetActive(true);

        nameText.text = contactData.contactName;
        messageText.text = contactData.contactMessage;
        UpdateProfileImage();

        if (affectionSlider != null)
        {
            bool hasAffection = !string.IsNullOrEmpty(contactData.characterName);
            affectionSlider.gameObject.SetActive(hasAffection);
            affectionSlider.interactable = false;
            if (hasAffection)
            {
                UpdateAffectionBar();
            }
        }
    }

    private void DisableContact()
    {
        contactRoot.SetActive(false);
    }

    private void UpdateProfileImage()
    {
        if (profileImage == null) return;
        Sprite newSprite = contactData.contactImage;
        if (contactData.profileImageStates != null)
        {
            foreach (var state in contactData.profileImageStates)
            {
                if (!string.IsNullOrEmpty(state.requiredFlag) && FlagManager.Instance.GetFlag(state.requiredFlag))
                {
                    newSprite = state.sprite;
                }
            }
        }
        profileImage.sprite = newSprite;
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