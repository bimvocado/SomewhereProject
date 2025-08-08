using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSlotButton : MonoBehaviour
{
    [SerializeField] private int slotIndex;
    [SerializeField] private TMP_Text slotInfoText;
    [SerializeField] private Button button;

    private void OnEnable()
    {
        UpdateSlotInfo();
    }

    public void UpdateSlotInfo()
    {
        GameData data = SaveLoadManager.Instance.GetSaveDataInfo(slotIndex);
        if (data != null)
        {
            slotInfoText.text = $"���� {slotIndex + 1}\n{data.playerLastName}{data.playerFirstName}";
        }
        else
        {
            slotInfoText.text = $"���� {slotIndex + 1}\n(�������)";
        }
    }

    public void OnSaveButtonClick()
    {
        SaveLoadManager.Instance.SaveGame(slotIndex);
        UpdateSlotInfo();
        Debug.Log($"{slotIndex}�� ���Կ� ���� �õ�");
    }

    public void OnLoadButtonClick()
    {
        SaveLoadManager.Instance.LoadGame(slotIndex);
        Debug.Log($"{slotIndex}�� ���Կ��� �ҷ����� �õ�");
    }
}