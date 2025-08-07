using UnityEngine;

public class PopupEventBridge : MonoBehaviour
{
    public void ShowTxtPopupEvt(string message)
    {
        if (PopupManager.Instance != null)
        {
            PopupManager.Instance.ShowTxtPopup(message);
        }
    }

    public void ShowImgPopupEvt(Sprite sprite)
    {
        if (PopupManager.Instance != null)
        {
            PopupManager.Instance.ShowImagePopup(sprite);
        }
    }

    public void HidePopupEvt()
    {
        if (PopupManager.Instance != null)
        {
            PopupManager.Instance.HidePopupEvent();
        }
    }
}
