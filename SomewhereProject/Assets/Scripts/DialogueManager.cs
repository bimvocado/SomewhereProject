using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static event Action OnDialogueAdvanced;
    public static DialogueManager Instance { get; private set; }

    [Header("Dialogue UI Prefab")]
    [SerializeField] private GameObject dialogueUIPrefab;

    [Header("UI Components")]
    [SerializeField] private GameObject go_DialogueBar;
    private GameObject go_NameBar;
    private TMP_Text txt_dialogue;
    private TMP_Text txt_name;
    private Transform choicePanel;
    private Image img_DialogueBackground;
    private Image img_NameBackground;
    private GameObject nextIndicator;


    private GameObject instantiatedDialogueUI;

    private DialogueData _currentDialogueData;
    private Dialogue[] currentDialogues;
    private int dialogueIndex = 0;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool isWaitingForChoiceClick = false;
    private string _fullyProcessedLine;
    private bool isLoadingNextDialogue = false;
    private bool isProcessingDisplayNext = false;
    private string _currentDialogueAssetKey;

    private float typingSpeed = 0.05f;
    private string playerLastName = "��";
    private string playerFirstName = "����";

    private float lastInteractionTime = 0f;
    private const float interactionCooldown = 0.2f;

    private bool isSkipping = false;
    private bool isAutoMode = false;
    private float autoModeTimer = 0f;
    [SerializeField] private float autoModeDelay = 2.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (dialogueUIPrefab != null)
        {
            instantiatedDialogueUI = Instantiate(dialogueUIPrefab);
            DontDestroyOnLoad(instantiatedDialogueUI);

            go_DialogueBar = instantiatedDialogueUI.transform.Find("UI_Dialogue/DialogueBar")?.gameObject;
            go_NameBar = instantiatedDialogueUI.transform.Find("UI_Dialogue/NameBar")?.gameObject;
            choicePanel = instantiatedDialogueUI.transform.Find("UI_Dialogue/ChoicePanel");
            nextIndicator = instantiatedDialogueUI.transform.Find("UI_Dialogue/NextIndicator")?.gameObject;

            if (go_DialogueBar != null)
            {
                txt_dialogue = go_DialogueBar.GetComponentInChildren<TMP_Text>();
                img_DialogueBackground = go_DialogueBar.GetComponentInChildren<Image>();
            }
            if (go_NameBar != null)
            {
                txt_name = go_NameBar.GetComponentInChildren<TMP_Text>();
                img_NameBackground = go_NameBar.GetComponentInChildren<Image>();
            }

            if (nextIndicator != null) nextIndicator.SetActive(false);
            if (choicePanel != null) choicePanel.gameObject.SetActive(false);
            if (go_DialogueBar != null) go_DialogueBar.SetActive(false);
            if (go_NameBar != null) go_NameBar.SetActive(false);
        }
        else
        {
            Debug.LogError("DialogueUI �������� DialogueManager�� �Ҵ� �ȵ�");
        }

        typingSpeed = PlayerPrefs.GetFloat("TypingSpeed", 0.05f);
        if (nextIndicator != null) nextIndicator.SetActive(false);
    }

    private void Update()
    {
        if (BarUIManager.Instance != null && BarUIManager.Instance.IsPhoneUIShowing())
        {
            return;
        }
        if (isAutoMode && !isTyping && !isWaitingForChoiceClick && go_DialogueBar.activeSelf)
        {
            autoModeTimer += Time.deltaTime;
            if (autoModeTimer >= autoModeDelay)
            {
                autoModeTimer = 0f;
                DisplayNext();
            }
        }
        else if (isSkipping)
        {
            bool isWaitingOnChoice = dialogueIndex > 0 && currentDialogues[dialogueIndex - 1].hasChoices;
            bool isUnreadDialogue = !ReadLogManager.Instance.IsRead(_currentDialogueAssetKey, dialogueIndex);

            if (isWaitingOnChoice || isUnreadDialogue || !go_DialogueBar.activeSelf)
            {
                isSkipping = false;
                return;
            }
            DisplayNext();
        }
        else
        {
            if (!go_DialogueBar.activeSelf || choicePanel.gameObject.activeSelf) return;
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current != null)
                {
                    PointerEventData eventData = new PointerEventData(EventSystem.current);
                    eventData.position = Input.mousePosition;
                    List<RaycastResult> results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(eventData, results);

                    bool clickedOnDialogueBar = false;
                    foreach (RaycastResult result in results)
                    {
                        if (result.gameObject == go_DialogueBar || result.gameObject.transform.IsChildOf(go_DialogueBar.transform))
                        {
                            clickedOnDialogueBar = true;
                            break;
                        }
                    }

                    if (clickedOnDialogueBar)
                    {
                        DisplayNext();
                    }
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") != 0f || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                DisplayNext();
            }
        }
    }

    public void StartSkip()
    {
        isSkipping = true;
        isAutoMode = false;
    }

    public void ToggleAuto(bool autoOn)
    {
        isAutoMode = autoOn;
        autoModeTimer = 0f;
        if (isAutoMode) isSkipping = false;
    }

    private void SetDialogueData(DialogueData data, string assetKey)
    {
        _currentDialogueData = data;
        _currentDialogueAssetKey = assetKey;
        currentDialogues = data.dialogues;
        dialogueIndex = 0;
        isWaitingForChoiceClick = false;
        isLoadingNextDialogue = false;

        SettingUI(true);
        DisplayNext();
    }

    public void StartDialogueFromReference(AssetReferenceT<DialogueData> assetRef)
    {
        if (assetRef == null || !assetRef.RuntimeKeyIsValid() || isLoadingNextDialogue) return;

        isLoadingNextDialogue = true;
        string key = assetRef.AssetGUID;

        assetRef.LoadAssetAsync<DialogueData>().Completed += handle =>
        {
            isLoadingNextDialogue = false;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                SetDialogueData(handle.Result, key);
            }
            else
            {
                Debug.LogError($"Dialogue load failed from AssetReference: {key}");
                EndDialogue();
            }
        };
    }

    public void DisplayNext()
    {
        if (isProcessingDisplayNext || isLoadingNextDialogue) return;

        try
        {
            isProcessingDisplayNext = true;

            if (Time.time - lastInteractionTime < interactionCooldown) return;

            if (isTyping)
            {
                FinishTyping();
                return;
            }

            OnDialogueAdvanced?.Invoke();
            if (nextIndicator != null) nextIndicator.SetActive(false);

            if (isWaitingForChoiceClick)
            {
                ShowChoices(currentDialogues[dialogueIndex - 1].choices);
                return;
            }

            ClearChoices();

            while (dialogueIndex < currentDialogues.Length)
            {
                Dialogue line = currentDialogues[dialogueIndex];

                if (string.IsNullOrEmpty(line.line))
                {
                    if (!string.IsNullOrEmpty(line.eventName)) EventManager.Instance.TriggerEvent(line.eventName);
                    dialogueIndex++;
                    continue;
                }

                if (!CheckCondition(line))
                {
                    dialogueIndex++;
                    continue;
                }

                SettingUI(true);
                ReadLogManager.Instance.MarkAsRead(_currentDialogueAssetKey, dialogueIndex);
                ApplyCharacterColors(line.speaker);

                if (line.speaker == "����")
                {
                    go_NameBar.SetActive(true);
                    txt_name.text = playerLastName + playerFirstName;
                }
                else if (string.IsNullOrEmpty(line.speaker))
                {
                    go_NameBar.SetActive(false);
                }
                else
                {
                    go_NameBar.SetActive(true);
                    txt_name.text = line.speaker;
                }

                _fullyProcessedLine = ProcessLinePlaceholders(line.line);

                if (!string.IsNullOrEmpty(line.eventName))
                {
                    if (line.eventName.StartsWith("CMD:"))
                    {
                        ProcessCommand(line.eventName);
                    }
                    else
                    {
                        EventManager.Instance.TriggerEvent(line.eventName);
                    }
                }

                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                typingCoroutine = StartCoroutine(TypeDialogue(_fullyProcessedLine));

                dialogueIndex++;

                if (line.hasChoices && line.choices.Length > 0)
                {
                    isWaitingForChoiceClick = true;
                }

                return;
            }

            EndDialogue();
        }
        finally
        {
            isProcessingDisplayNext = false;
        }
    }

    private string ProcessLinePlaceholders(string line)
    {
        string processedLine = line;
        if (processedLine.Contains("[����"))
        {
            processedLine = processedLine.Replace("[���ִ�]", KoreanPostpositionHelper.Josa(playerFirstName, "�̴�/��"));
            processedLine = processedLine.Replace("[���ְ�]", KoreanPostpositionHelper.Josa(playerFirstName, "�̰�/��"));
            processedLine = processedLine.Replace("[���ָ�]", KoreanPostpositionHelper.Josa(playerFirstName, "�̸�/��"));
            processedLine = processedLine.Replace("[���־�]", KoreanPostpositionHelper.Josa(playerFirstName, "��/��"));
            processedLine = processedLine.Replace("[���ֿ���]", KoreanPostpositionHelper.Josa(playerFirstName, "�̿���/����"));
            processedLine = processedLine.Replace("[���ְ�]", KoreanPostpositionHelper.Josa(playerFirstName, "�̰�/��"));
            processedLine = processedLine.Replace("[���ֵ�]", KoreanPostpositionHelper.Josa(playerFirstName, "�̵�/��"));
            processedLine = processedLine.Replace("[���ֶ��]", KoreanPostpositionHelper.Josa(playerFirstName, "�̶��/���"));
            processedLine = processedLine.Replace("[������]", KoreanPostpositionHelper.Josa(playerFirstName, "��/"));
            processedLine = processedLine.Replace("[����]", playerFirstName);
            processedLine = processedLine.Replace("[�����̸��̿���]", KoreanPostpositionHelper.Josa(playerLastName + playerFirstName, "�̿���/����"));
            processedLine = processedLine.Replace("[�����̸��̰�]", KoreanPostpositionHelper.Josa(playerLastName + playerFirstName, "�̰�/��"));
            processedLine = processedLine.Replace("[�����̸�]", playerLastName + playerFirstName);
        }
        return processedLine;
    }

    private void FinishTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        txt_dialogue.text = _fullyProcessedLine;
        isTyping = false;
        lastInteractionTime = Time.time;
        if (nextIndicator != null) nextIndicator.SetActive(true);
    }

    private IEnumerator TypeDialogue(string line)
    {
        isTyping = true;
        txt_dialogue.text = "";
        foreach (char letter in line.ToCharArray())
        {
            txt_dialogue.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        typingCoroutine = null;
        lastInteractionTime = Time.time;

        if (!isWaitingForChoiceClick && nextIndicator != null)
        {
            nextIndicator.SetActive(true);
        }
    }

    void ShowChoices(Choice[] choices)
    {
        go_DialogueBar.gameObject.SetActive(false);
        go_NameBar.gameObject.SetActive(false);
        if (nextIndicator != null) nextIndicator.SetActive(false);
        choicePanel.gameObject.SetActive(true);

        foreach (Choice choice in choices)
        {
            GameObject buttonGO = ObjectPooler.Instance.SpawnFromPool("ChoiceButton", Vector3.zero, Quaternion.identity);
            if (buttonGO == null) continue;

            buttonGO.transform.SetParent(choicePanel, false);
            buttonGO.transform.localScale = Vector3.one;

            var button = buttonGO.GetComponent<Button>();
            var buttonText = buttonGO.GetComponentInChildren<TMP_Text>();
            string choiceText = choice.text;

            bool conditionMet = CheckChoiceCondition(choice);
            bool coinsMet = CoinManager.Instance.PlayerCoin >= choice.requiredCoin;

            button.interactable = conditionMet && coinsMet;

            if (!conditionMet)
            {
                choiceText += $" ( {choice.conditionTargetCharacter}�� ȣ���� ����, �������� �������� �����ϼ���.)";
            }
            else if (!coinsMet)
            {
                choiceText += $" ({choice.requiredCoin} ���� �ʿ�)";
            }

            buttonText.text = choiceText;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnChoiceSelected(choice));
        }
    }

    void OnChoiceSelected(Choice choice)
    {
        if (isLoadingNextDialogue) return;

        if (choice.requiredCoin > 0)
        {
            if (!CoinManager.Instance.SpendCoin(choice.requiredCoin))
            {
                return;
            }
        }

        ClearChoices();

        if (choice.resetsAffection)
        {
            AffectionManager.Instance.ResetAffection(choice.targetCharacterForAffection);
        }
        else
        {
            AffectionManager.Instance.ChangeAffection(choice.targetCharacterForAffection, choice.affectionChange);
        }

        if (!string.IsNullOrEmpty(choice.flagToSet))
        {
            FlagManager.Instance.SetFlag(choice.flagToSet, choice.flagValue);
        }

        if (choice.nextDialogue != null && choice.nextDialogue.RuntimeKeyIsValid())
        {
            StartDialogueFromReference(choice.nextDialogue);
        }
        else if (!isLoadingNextDialogue)
        {
            SettingUI(true);
            DisplayNext();
        }
    }

    private void EndDialogue()
    {
        if (isLoadingNextDialogue) return;

        SettingUI(false);
        if (_currentDialogueData != null && _currentDialogueData.nextDialogueOnCompletion.RuntimeKeyIsValid())
        {
            StartDialogueFromReference(_currentDialogueData.nextDialogueOnCompletion);
        }
        else
        {
            Debug.Log("��ȭ ����");
        }
    }

    private void ClearChoices()
    {
        choicePanel.gameObject.SetActive(false);
        foreach (Transform child in choicePanel)
        {
            ObjectPooler.Instance.ReturnToPool("ChoiceButton", child.gameObject);
        }
    }

    private void SettingUI(bool active)
    {
        go_DialogueBar.SetActive(active);
        go_NameBar.SetActive(active);

        if (!active)
        {
            ClearChoices();
        }
    }

    private void ApplyCharacterColors(string speaker)
    {
        if (CharacterColorManager.Instance == null) return;
        CharacterColorData colorData = CharacterColorManager.Instance.GetColorData(speaker);

        if (img_DialogueBackground != null) img_DialogueBackground.color = colorData.backgroundColor;
        if (img_NameBackground != null) img_NameBackground.color = colorData.backgroundColor;

        if (txt_name != null) txt_name.color = colorData.textColor;
        if (txt_dialogue != null) txt_dialogue.color = colorData.textColor;
    }

    private bool CheckCondition(Dialogue d)
    {
        switch (d.conditionType)
        {
            case ConditionType.None:
                return true;
            case ConditionType.FlagTrue:
                return FlagManager.Instance.GetFlag(d.conditionKey);
            case ConditionType.FlagFalse:
                return !FlagManager.Instance.GetFlag(d.conditionKey);
            case ConditionType.AffectionGreater:
                return AffectionManager.Instance.GetAffection(d.conditionTargetCharacter) > d.conditionValue;
            case ConditionType.AffectionLess:
                return AffectionManager.Instance.GetAffection(d.conditionTargetCharacter) < d.conditionValue;
            case ConditionType.AffectionEqual:
                return AffectionManager.Instance.GetAffection(d.conditionTargetCharacter) == d.conditionValue;
            case ConditionType.FlagFalseAndAffectionGreater:
                return !FlagManager.Instance.GetFlag(d.conditionKey) && AffectionManager.Instance.GetAffection(d.conditionTargetCharacter) > d.conditionValue;
            case ConditionType.FlagFalseAndAffectionLess:
                return !FlagManager.Instance.GetFlag(d.conditionKey) && AffectionManager.Instance.GetAffection(d.conditionTargetCharacter) < d.conditionValue;
            case ConditionType.FlagTrueAndAffectionGreater:
                return FlagManager.Instance.GetFlag(d.conditionKey) && AffectionManager.Instance.GetAffection(d.conditionTargetCharacter) > d.conditionValue;
            case ConditionType.FlagTrueAndAffectionLess:
                return FlagManager.Instance.GetFlag(d.conditionKey) && AffectionManager.Instance.GetAffection(d.conditionTargetCharacter) < d.conditionValue;
        }
        return true;
    }

    private bool CheckChoiceCondition(Choice choice)
    {
        switch (choice.conditionType)
        {
            case ConditionType.None:
                return true;
            case ConditionType.FlagTrue:
                return FlagManager.Instance.GetFlag(choice.conditionKey);
            case ConditionType.FlagFalse:
                return !FlagManager.Instance.GetFlag(choice.conditionKey);
            case ConditionType.AffectionGreater:
                return AffectionManager.Instance.GetAffection(choice.targetCharacterForAffection) > choice.conditionValue;
            case ConditionType.AffectionLess:
                return AffectionManager.Instance.GetAffection(choice.targetCharacterForAffection) < choice.conditionValue;
            case ConditionType.AffectionEqual:
                return AffectionManager.Instance.GetAffection(choice.targetCharacterForAffection) == choice.conditionValue;
            case ConditionType.AllCharacterAffectionLess:
                if (string.IsNullOrEmpty(choice.targetCharacterForAffection)) return true;
                string[] characters = choice.conditionTargetCharacter.Split(',');
                foreach (string character in characters)
                {
                    if (AffectionManager.Instance.GetAffection(character.Trim()) <= choice.conditionValue) { return false; }
                }
                return true;
        }
        return true;
    }

    public void GetCurrentDialogueState(out string assetKey, out int index)
    {
        assetKey = _currentDialogueAssetKey;
        index = this.dialogueIndex;
    }

    public void LoadDialogueState(string assetKey, int index)
    {
        if (string.IsNullOrEmpty(assetKey))
        {
            EndDialogue();
            return;
        }

        isLoadingNextDialogue = true;
        Addressables.LoadAssetAsync<DialogueData>(assetKey).Completed += handle =>
        {
            isLoadingNextDialogue = false;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                SetDialogueData(handle.Result, assetKey);
                this.dialogueIndex = index;
                DisplayNext();
            }
            else
            {
                Debug.LogError($"Dialogue load failed from asset key: {assetKey}");
                EndDialogue();
            }
        };
    }

    public void SetPlayerName(string firstName, string lastName)
    {
        playerFirstName = firstName;
        playerLastName = lastName;
    }

    public void SetTypingSpeed(float newSpeed)
    {
        typingSpeed = Mathf.Max(0.01f, newSpeed);
        PlayerPrefs.SetFloat("TypingSpeed", typingSpeed);
        PlayerPrefs.Save();
    }

    private void ProcessCommand(string command)
    {
        string[] parts = command.Split(':');
        if (parts.Length < 2)
        {
            Debug.LogWarning($"�߸��� ��ɾ� ����: {command}");
            return;
        }

        string commandType = parts[1];

        switch (commandType)
        {
            case "SetFlag":
                if (parts.Length > 3)
                {
                    string flagKey = parts[2];
                    if (bool.TryParse(parts[3], out bool flagValue))
                    {
                        FlagManager.Instance.SetFlag(flagKey, flagValue);
                        Debug.Log($"�÷��� ����: {flagKey} = {flagValue}");
                    }
                }
                break;

            case "ResetAffection":
                if (parts.Length > 2)
                {
                    string target = parts[2];
                    AffectionManager.Instance.ResetAffection(target);
                }
                break;

            default:
                Debug.LogWarning($"�� �� ���� ��ɾ��Դϴ�: {commandType}");
                break;
        }
    }
}