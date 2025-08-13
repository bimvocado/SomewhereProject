using UnityEngine;

public class GameProgressionManager : MonoBehaviour
{
    public static GameProgressionManager Instance { get; private set; }

    private const string PlaythroughCompleteKey = "PlaythroughComplete";
    private const string SlotUnlockKeyPrefix = "SaveSlotUnlocked_";

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
        Debug.Log("1ȸ�� �÷��� �Ϸ�");
    }

    public bool IsPastFirstPlaythrough()
    {
        return PlayerPrefs.GetInt(PlaythroughCompleteKey, 0) == 1;
    }

    public void UnlockSlot(int slotIndex)
    {
        PlayerPrefs.SetInt(SlotUnlockKeyPrefix + slotIndex, 1);
        PlayerPrefs.Save();
        Debug.Log($"���̺� ���� {slotIndex + 1} ����");
    }

    public bool IsSlotUnlocked(int slotIndex)
    {
        if (slotIndex == 0) return true;
        return PlayerPrefs.GetInt(SlotUnlockKeyPrefix + slotIndex, 0) == 1;
    }
}