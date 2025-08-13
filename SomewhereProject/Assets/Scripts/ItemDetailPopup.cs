using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ItemDetailPopup : MonoBehaviour
{
    public static ItemDetailPopup Instance { get; private set; }

    [Header("UI 컴포넌트 연결")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Image itemIconImage;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private TMP_Text itemPriceText;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private Button closeButton;

    [Header("색상 변경 파트 연결")]
    [SerializeField] private Image[] colorableParts;

    private ItemData currentItemData;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        if (purchaseButton != null) purchaseButton.onClick.AddListener(OnPurchaseButtonClick);
        if (closeButton != null) closeButton.onClick.AddListener(Hide);

        if (popupPanel != null) popupPanel.SetActive(false);
    }

    public void Show(ItemData data)
    {
        if (data == null) return;
        currentItemData = data;

        itemIconImage.sprite = currentItemData.itemIcon;
        itemNameText.text = currentItemData.itemName;
        itemDescriptionText.text = currentItemData.itemDescription;
        itemPriceText.text = currentItemData.price.ToString();

        ApplyCharacterColor(currentItemData);

        popupPanel.SetActive(true);
    }

    public void Hide()
    {
        popupPanel.SetActive(false);
    }

    private void OnPurchaseButtonClick()
    {
        if (currentItemData == null) return;

        if (CoinManager.Instance != null && CoinManager.Instance.SpendCoin(currentItemData.price))
        {
            ApplyItemEffect(currentItemData);

            Hide();
        }
        else
        {
            Debug.LogWarning("코인이 부족하여 아이템을 구매할 수 없습니다.");
        }
    }

    private void ApplyItemEffect(ItemData data)
    {
        if (!string.IsNullOrEmpty(data.affectionChange.targetCharacter) && data.affectionChange.changeValue != 0)
        {
            if (AffectionManager.Instance != null)
            {
                AffectionManager.Instance.ChangeAffection(data.affectionChange.targetCharacter, data.affectionChange.changeValue);
            }
        }
    }

    private void ApplyCharacterColor(ItemData data)
    {
        if (!string.IsNullOrEmpty(data.affectionChange.targetCharacter) && colorableParts != null && colorableParts.Length > 0)
        {
            if (CharacterColorManager.Instance != null)
            {
                CharacterColorData colorData = CharacterColorManager.Instance.GetColorData(data.affectionChange.targetCharacter);
                if (colorData != null)
                {
                    foreach (Image part in colorableParts)
                    {
                        if (part != null)
                        {
                            part.color = colorData.backgroundColor;
                        }
                    }
                }
            }
        }
        else
        {
            foreach (Image part in colorableParts)
            {
                if (part != null)
                {
                    part.color = Color.white;
                }
            }
        }
    }
}