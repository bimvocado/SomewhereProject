using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailPopup : MonoBehaviour
{
    public static ItemDetailPopup Instance { get; private set; }

    [Header("UI 컴포넌트")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Image itemIconImage;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private TMP_Text itemPriceText;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Image[] colorableParts;


    private ItemData currentItemData;
    private ShopItemButton lastClickedButton;

    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }
        if (purchaseButton != null) purchaseButton.onClick.AddListener(OnPurchaseButtonClick);
        if (closeButton != null) closeButton.onClick.AddListener(Hide);
        if (popupPanel != null) popupPanel.SetActive(false);
    }

    public void Show(ItemData data, ShopItemButton clickedButton)
    {
        if (data == null) return;
        currentItemData = data;
        lastClickedButton = clickedButton;

        itemIconImage.sprite = currentItemData.itemIcon;
        itemNameText.text = currentItemData.itemName;
        itemDescriptionText.text = currentItemData.itemDescription;
        itemPriceText.text = $"{currentItemData.price} 코인";
        ApplyCharacterColor(currentItemData);

        popupPanel.SetActive(true);
    }

    public void Hide()
    {
        popupPanel.SetActive(false);

        if (lastClickedButton != null)
        {
            lastClickedButton.UpdateDisplay();
        }
    }

    private void OnPurchaseButtonClick()
    {
        if (currentItemData == null) return;
        string mainMessage = $"{currentItemData.itemName}을(를) 구매하시겠습니까?";
        string costMessage = $"<color=yellow>{currentItemData.price} 코인</color>이 차감됩니다.";
        if (ConfirmationPopup.Instance != null)
        {
            ConfirmationPopup.Instance.Show(mainMessage, costMessage, () => {
                if (CoinManager.Instance != null && CoinManager.Instance.SpendCoin(currentItemData.price))
                {
                    ApplyItemEffect(currentItemData);
                    if (GameProgressionManager.Instance != null)
                    {
                        GameProgressionManager.Instance.MarkItemAsPurchased(currentItemData.itemID);
                    }
                    Hide();
                }
            });
        }
    }
    private void ApplyItemEffect(ItemData data)
    {
        if (!string.IsNullOrEmpty(data.affectionChange.targetCharacter) && data.affectionChange.changeValue != 0)
        {
            if (AffectionManager.Instance != null) { AffectionManager.Instance.ChangeAffection(data.affectionChange.targetCharacter, data.affectionChange.changeValue); }
        }
    }
    private void ApplyCharacterColor(ItemData data)
    {
        if (!string.IsNullOrEmpty(data.affectionChange.targetCharacter) && colorableParts != null && colorableParts.Length > 0)
        {
            if (CharacterColorManager.Instance != null)
            {
                CharacterColorData colorData = CharacterColorManager.Instance.GetColorData(data.affectionChange.targetCharacter);
                if (colorData != null) { foreach (Image part in colorableParts) { if (part != null) part.color = colorData.backgroundColor; } }
            }
        }
        else { foreach (Image part in colorableParts) { if (part != null) part.color = Color.white; } }
    }
}