using UnityEngine;

public class BarUIManager : MonoBehaviour
{
    public static BarUIManager Instance { get; private set; }

    [SerializeField]
    private GameObject backPanel;
    [SerializeField]
    private GameObject phone;

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

    public void HidePhone() { 
        if (phone != null)
        {
            phoneAnimator.SetTrigger("Hide");
        }
        if (backPanel != null)
        {
            backPanel.SetActive(false);
        }
    }

    public bool IsPhoneUIShowing()
    {
        return backPanel != null && backPanel.activeSelf;
    }
}
