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
            return null;
        }

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