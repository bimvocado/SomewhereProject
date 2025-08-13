using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemButton : MonoBehaviour
{
    [Header("아이템 데이터 연결")]
    public ItemData itemData;

    [Header("UI 컴포넌트 연결")]
    [SerializeField] private TMP_Text itemPriceText;

    private Button buttonComponent;

    void Awake()
    {
        buttonComponent = GetComponent<Button>();
    }

    void OnEnable()
    {
        UpdateDisplay();
        GameProgressionManager.OnItemPurchased += HandleItemPurchased;
    }

    void OnDisable()
    {
        GameProgressionManager.OnItemPurchased -= HandleItemPurchased;
    }

    private void HandleItemPurchased(string purchasedItemID)
    {
        if (itemData == null) return;

        if (itemData.itemID == purchasedItemID)
        {
            UpdateDisplay();
        }
    }

    public void UpdateDisplay()
    {
        if (itemData == null) return;

        bool isPurchased = GameProgressionManager.Instance.HasItemBeenPurchased(itemData.itemID);

        if (isPurchased)
        {
            itemPriceText.text = "판매 완료";
            buttonComponent.interactable = false;
        }
        else
        {
            itemPriceText.text = itemData.price.ToString();
            buttonComponent.interactable = true;
        }
    }

    public void OnClick_ShowDetailPopup()
    {
        if (itemData == null)
        {
            Debug.LogError("ShopItemButton에 ItemData가 연결되지 않았습니다!");
            return;
        }

        if (ItemDetailPopup.Instance != null)
        {
            ItemDetailPopup.Instance.Show(itemData, this);
        }
    }
}