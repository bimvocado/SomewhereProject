using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemButton : MonoBehaviour
{
    [Header("������ ������ ����")]
    public ItemData itemData;

    [Header("UI ������Ʈ ����")]
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
            itemPriceText.text = "�Ǹ� �Ϸ�";
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
            Debug.LogError("ShopItemButton�� ItemData�� ������� �ʾҽ��ϴ�!");
            return;
        }

        if (ItemDetailPopup.Instance != null)
        {
            ItemDetailPopup.Instance.Show(itemData, this);
        }
    }
}