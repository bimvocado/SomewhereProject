using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    [SerializeField] private GameObject popupUI;

    private GameObject insPopup;
    private PopupController popupController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (popupUI != null)
            {
                insPopup = Instantiate(popupUI);
                DontDestroyOnLoad(insPopup);

                popupController = insPopup.GetComponent<PopupController>();
                if (popupController == null)
                {
                    Debug.Log("PopupController¾øÀ½");
                }

                insPopup.SetActive(false);
            }
        }
        else Destroy(gameObject);
    }

    public void ShowPopup(string text = "", Sprite sprite = null)
    {
        if (insPopup != null && popupController != null)
        {
            popupController.SetText(text);
            popupController.SetImage(sprite);
            insPopup.SetActive(true);
        }
    }


    public void HidePopup()
    {
        if (insPopup != null)
        {
            insPopup.SetActive(false);
        }
    }

    public void ShowTxtPopup(string text)
    {
        ShowPopup(text);
    }

    public void ShowImagePopup(Sprite sprite)
    {
        ShowPopup("", sprite);
    }

    public void HidePopupEvent()
    {
        HidePopup();
    }

}
