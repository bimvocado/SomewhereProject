using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSlotButton : MonoBehaviour
{
    [SerializeField] private int slotIndex;
    [SerializeField] private TMP_Text slotInfoText;
    [SerializeField] private Button mainButton;

    [SerializeField] private TMP_Text timestampText;

    [Header("삭제 버튼")]
    [SerializeField] private GameObject deleteButtonObject;

    [Header("상태별 색상 설정")]
    [SerializeField] private Image backgroundGlowImage;
    [SerializeField] private Color emptyColor = Color.grey;
    [SerializeField] private Color savedColor = Color.cyan;

    [Header("슬롯 코인 설정")]
    [SerializeField] private int unlockCost = 100;

    private bool isLockedForPurchase = false;

    private void OnEnable()
    {
        UpdateSlotInfo();
    }

    public void UpdateSlotInfo()
    {
        if (GameProgressionManager.Instance == null || SaveLoadManager.Instance == null) return;

        isLockedForPurchase = false;
        mainButton.interactable = true;
        if (backgroundGlowImage != null) backgroundGlowImage.color = emptyColor;
        timestampText?.gameObject.SetActive(false);

        if (slotIndex > 0 && !GameProgressionManager.Instance.IsPastFirstPlaythrough())
        {
            slotInfoText.text = $"슬롯 {slotIndex + 1}\n(2회차부터 사용 가능)";
            mainButton.interactable = false;
            deleteButtonObject?.SetActive(false);
            return;
        }

        if (!GameProgressionManager.Instance.IsSlotUnlocked(slotIndex))
        {
            slotInfoText.text = $"슬롯 {slotIndex + 1}\n({unlockCost} 코인으로 열기)";
            isLockedForPurchase = true;
            deleteButtonObject?.SetActive(false);
        }
        else
        {
            GameData data = SaveLoadManager.Instance.GetSaveDataInfo(slotIndex);
            if (data != null)
            {
                slotInfoText.text = $"슬롯 {slotIndex + 1}\n{data.playerLastName}{data.playerFirstName}";
                deleteButtonObject?.SetActive(true);
                if (backgroundGlowImage != null) backgroundGlowImage.color = savedColor;

                if (timestampText != null)
                {
                    timestampText.gameObject.SetActive(true);
                    timestampText.text = data.saveTimestamp;
                }
            }
            else
            {
                slotInfoText.text = $"슬롯 {slotIndex + 1}\n(비어있음)";
                deleteButtonObject?.SetActive(false);
            }
        }
    }

    public void OnSaveButtonClick()
    {
        if (isLockedForPurchase) { TryUnlockSlot(); }
        else { SaveLoadManager.Instance.SaveGame(slotIndex); UpdateSlotInfo(); }
    }

    public void OnLoadButtonClick()
    {
        if (isLockedForPurchase) { TryUnlockSlot(); }
        else { SaveLoadManager.Instance.LoadGame(slotIndex); }
    }

    public void OnDeleteButtonClick()
    {
        if (SaveLoadManager.Instance != null)
        {
            SaveLoadManager.Instance.DeleteSaveData(slotIndex);
            UpdateSlotInfo();
        }
    }

    private void TryUnlockSlot()
    {
        if (CoinManager.Instance != null && CoinManager.Instance.SpendCoin(unlockCost))
        {
            GameProgressionManager.Instance.UnlockSlot(slotIndex);
            UpdateSlotInfo();
        }
        else
        {
            Debug.Log("코인이 부족하여 슬롯을 해금할 수 없습니다.");
        }
    }
}