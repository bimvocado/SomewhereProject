using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupController : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private Image contentImage;

    private void OnEnable()
    {
        DialogueManager.OnDialogueAdvanced += ClosePopup;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueAdvanced -= ClosePopup;
    }

    public void ShowPopup()
    {
        gameObject.SetActive(true);
    }

    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }

    public void SetText(string text)
    {
        if (contentText != null)
        {
            contentText.gameObject.SetActive(!string.IsNullOrEmpty(text));
            contentText.text = text;
        }
    }

    public void SetImage(Sprite sprite)
    {
        if (contentImage != null)
        {
            contentImage.gameObject.SetActive(sprite != null);
            contentImage.sprite = sprite;
        }
    }
}