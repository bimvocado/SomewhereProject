using UnityEngine;

public class BarUIManager : MonoBehaviour
{
    public static BarUIManager Instance { get; private set; }

    [SerializeField]
    private GameObject backPanel;
    public GameObject phone;


    [SerializeField]
    private GameObject applicationsContainer;

    private Animator phoneAnimator;
    private CanvasGroup canvasGroup;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                Debug.LogError("BarUIManager에 CanvasGroup 컴포넌트가 없습니다");
            }
            if (phone != null)
            {
                phoneAnimator = phone.GetComponent<Animator>();
                phone.SetActive(true);
            }
            if (backPanel != null)
            {
                backPanel.SetActive(false);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void ShowPhone()
    {
        if (backPanel != null)
        {
            backPanel.SetActive(true);
        }
        if (phoneAnimator != null)
        {
            phoneAnimator.SetTrigger("Phone");
        }

    }


    public void HidePhone()
    {
        if (applicationsContainer != null)
        {
            applicationsContainer.SetActive(false);
        }

        if (phoneAnimator != null)
        {
            phoneAnimator.SetTrigger("Hide");
        }
        if (backPanel != null)
        {
            backPanel.SetActive(false);
        }
        if (ItemDetailPopup.Instance != null)
        {
            ItemDetailPopup.Instance.Hide();
        }
        if (ConfirmationPopup.Instance != null)
        {
            ConfirmationPopup.Instance.ClosePopup();
        }

    }

    public void ShowApp(GameObject appPanelToShow)
    {
        if (applicationsContainer == null || appPanelToShow == null) return;

        applicationsContainer.SetActive(true);

        foreach (Transform app in applicationsContainer.transform)
        {
            app.gameObject.SetActive(false);
        }

        appPanelToShow.SetActive(true);
    }

    public void OnBackPanelClick()
    {
        HidePhone();
    }

    public bool IsPhoneUIShowing()
    {
        return backPanel != null && backPanel.activeSelf;
    }

    public void SetBarUIVisibility(bool isVisible)
    {
        if (canvasGroup == null) return;

        if (isVisible)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true; 
        }
        else
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void RefreshContacts()
    {
        if (applicationsContainer == null) return;

        ContactManager contactManager = applicationsContainer.GetComponentInChildren<ContactManager>(true);
        if (contactManager != null)
        {
            contactManager.UpdateAllContactsStatus();
            Debug.Log("<color=green>연락처 목록 갱신 성공</color>");
        }
    }
}