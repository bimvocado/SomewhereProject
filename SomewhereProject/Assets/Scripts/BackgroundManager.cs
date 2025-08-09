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
}