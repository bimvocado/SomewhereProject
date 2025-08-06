using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUI_Registrar : MonoBehaviour
{
    public GameObject dialogueBar;
    public GameObject nameBar;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public Transform choicePanel;
    public Image dialogueBackground;
    public Image nameBackground;
    public GameObject nextIndicator;

    private void Awake()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.RegisterDialogueUI(this);
        }
    }
}