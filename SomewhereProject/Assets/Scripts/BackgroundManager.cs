using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance { get; private set; }

    private BackgroundController activeBackgroundController;

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

    public void RegisterController(BackgroundController controller)
    {
        activeBackgroundController = controller;
    }

    public void ChangeBackground(Sprite newSprite)
    {
        if (activeBackgroundController != null)
        {
            activeBackgroundController.ChangeBackground(newSprite);
        }
        else
        {
            Debug.LogWarning("활성화된 BackgroundController 없음");
        }
    }

    public string GetCurrentBackgroundName()
    {
        if (activeBackgroundController == null)
        {
            Debug.LogWarning("진단 정보: GetCurrentBackgroundName()이 null을 반환합니다.");
            Debug.LogWarning("원인: BackgroundController가 BackgroundManager에게 등록(Register)된 적이 없습니다. Script Execution Order 문제일 가능성이 가장 높습니다.");
            return null;
        }

        Debug.Log("3차 진단 통과: activeBackgroundController가 정상적으로 등록되어 있습니다.");
        return activeBackgroundController.currentBackgroundName;
    }

    public void ChangeBackgroundByAddress(string address)
    {
        if (string.IsNullOrEmpty(address)) return;

        var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<Sprite>(address);
        handle.Completed += (op) =>
        {
            if (op.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                ChangeBackground(op.Result);
            }
            else
            {
                Debug.LogError($"배경 에셋 불러오기 실패");
            }
        };
    }
}