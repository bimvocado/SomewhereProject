using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemButton : MonoBehaviour
{
    [Header("������ ������ ����")]
    public ItemData itemData;
    [SerializeField] private TMP_Text itemPriceText;


    void Start()
    {
        if (itemData != null)
        {
            if (itemPriceText != null)
            {
                itemPriceText.text = itemData.price.ToString();
            }
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
            ItemDetailPopup.Instance.Show(itemData);
        }
    }
}