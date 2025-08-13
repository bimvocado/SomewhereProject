using System;
using UnityEngine;

public class GameProgressionManager : MonoBehaviour
{
    public static GameProgressionManager Instance { get; private set; }

    private const string PlaythroughCompleteKey = "PlaythroughComplete";
    private const string SlotUnlockKeyPrefix = "SaveSlotUnlocked_";
    private const string ItemPurchasedKeyPrefix = "ItemPurchased_";
    public static event Action<string> OnItemPurchased;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MarkFirstPlaythroughComplete()
    {
        PlayerPrefs.SetInt(PlaythroughCompleteKey, 1);
        PlayerPrefs.Save();
        Debug.Log("1회차 플레이 완료");
    }

    public bool IsPastFirstPlaythrough()
    {
        return PlayerPrefs.GetInt(PlaythroughCompleteKey, 0) == 1;
    }

    public void UnlockSlot(int slotIndex)
    {
        PlayerPrefs.SetInt(SlotUnlockKeyPrefix + slotIndex, 1);
        PlayerPrefs.Save();
        Debug.Log($"세이브 슬롯 {slotIndex + 1} 해제");
    }

    public bool IsSlotUnlocked(int slotIndex)
    {
        if (slotIndex == 0) return true;
        return PlayerPrefs.GetInt(SlotUnlockKeyPrefix + slotIndex, 0) == 1;
    }

    public void MarkItemAsPurchased(string itemID)
    {
        if (string.IsNullOrEmpty(itemID)) return;
        PlayerPrefs.SetInt("ItemPurchased_" + itemID, 1);
        PlayerPrefs.Save();
        Debug.Log($"<color=yellow>아이템 구매 기록 저장: {itemID}</color>");
        OnItemPurchased?.Invoke(itemID);
    }


    public bool HasItemBeenPurchased(string itemID)
    {
        if (string.IsNullOrEmpty(itemID)) return false;
        return PlayerPrefs.GetInt(ItemPurchasedKeyPrefix + itemID, 0) == 1;
    }
}