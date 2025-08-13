using UnityEngine;

public class BarUIManager : MonoBehaviour
{
    public static BarUIManager Instance { get; private set; }

    [SerializeField]
    private GameObject backPanel;
    [SerializeField]
    public GameObject phone;


    [SerializeField]
    private GameObject applicationsContainer;

    private Animator phoneAnimator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
}