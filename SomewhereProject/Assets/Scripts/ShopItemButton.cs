using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemButton : MonoBehaviour
{
    [Header("아이템 데이터 연결")]
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
            Debug.LogError("ShopItemButton에 ItemData가 연결되지 않았습니다!");
            return;
        }

        if (ItemDetailPopup.Instance != null)
        {
            ItemDetailPopup.Instance.Show(itemData);
        }
    }
}