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
            slotInfoText.text = $"슬롯 {slotIndex + 1}\n{data.playerLastName}{data.playerFirstName}";
        }
        else
        {
            slotInfoText.text = $"슬롯 {slotIndex + 1}\n(비어있음)";
        }
    }

    public void OnSaveButtonClick()
    {
        SaveLoadManager.Instance.SaveGame(slotIndex);
        UpdateSlotInfo();
        Debug.Log($"{slotIndex}번 슬롯에 저장 시도");
    }

    public void OnLoadButtonClick()
    {
        SaveLoadManager.Instance.LoadGame(slotIndex);
        Debug.Log($"{slotIndex}번 슬롯에서 불러오기 시도");
    }
}