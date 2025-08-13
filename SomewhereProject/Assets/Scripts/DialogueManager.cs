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
    private GameObject choiceBlockerPanel;

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
    private AsyncOperationHandle<DialogueData> _currentDialogueHandle;

    private float typingSpeed = 0.05f;
    private string playerLastName = "김";
    private string playerFirstName = "여주";

    private float lastInteractionTime = 0f;
    private const float interactionCooldown = 0.2f;

    private bool isSkipping = false;
    private bool isAutoMode = false;
    private bool wasAutoModeActiveBeforeChoice = false;
    private float autoModeTimer = 0f;
    [SerializeField] private float autoModeDelay = 0.05f;

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

            Transform dialogueBarTransform = instantiatedDialogueUI.transform.Find("UI_Dialogue/DialogueBar");
            if (dialogueBarTransform != null)
            {
                go_DialogueBar = dialogueBarTransform.gameObject;
                txt_dialogue = go_DialogueBar.GetComponentInChildren<TMP_Text>();
                img_DialogueBackground = go_DialogueBar.GetComponentInChildren<Image>();
            }

            Transform nameBarTransform = instantiatedDialogueUI.transform.Find("UI_Dialogue/NameBar");
            if (nameBarTransform != null)
            {
                go_NameBar = nameBarTransform.gameObject;
                txt_name = go_NameBar.GetComponentInChildren<TMP_Text>();
                img_NameBackground = go_NameBar.GetComponentInChildren<Image>();
            }

            Transform choicePanelTransform = instantiatedDialogueUI.transform.Find("UI_Dialogue/ChoicePanel");
            if (choicePanelTransform != null)
            {
                choicePanel = choicePanelTransform;
            }

            Transform nextIndicatorTransform = instantiatedDialogueUI.transform.Find("UI_Dialogue/nextIndicator");
            if (nextIndicatorTransform != null)
            {
                nextIndicator = nextIndicatorTransform.gameObject;
            }

            Transform blockerPanelTransform = instantiatedDialogueUI.transform.Find("UI_Dialogue/choiceblockpanel");
            if (blockerPanelTransform != null)
            {
                choiceBlockerPanel = blockerPanelTransform.gameObject;
            }


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

            if (choiceBlockerPanel != null) choiceBlockerPanel.SetActive(false);
            if (nextIndicator != null) nextIndicator.SetActive(false);
            if (choicePanel != null) choicePanel.gameObject.SetActive(false);
            if (go_DialogueBar != null) go_DialogueBar.SetActive(false);
            if (go_NameBar != null) go_NameBar.SetActive(false);
        }
        else
        {
            Debug.LogError("DialogueUI 프리팹 DialogueManager에 할당 안됨");
        }

        typingSpeed = PlayerPrefs.GetFloat("TypingSpeed", 0.05f);
        autoModeDelay = PlayerPrefs.GetFloat("AutoModeDelay", 0.5f);

        if (nextIndicator != null) nextIndicator.SetActive(false);
    }

    private void Update()
    {
        if (choicePanel != null && choicePanel.gameObject.activeSelf)
        {
            return;
        }

        if ((BarUIManager.Instance != null && BarUIManager.Instance.IsPhoneUIShowing()) || (SaveNSettingUI.Instance != null && SaveNSettingUI.Instance.IsUIShowing()))
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
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                PointerEventData eventData = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);

                bool clickedOnDialogueArea = false;
                foreach (var result in results)
                {
                    if (result.gameObject == go_DialogueBar || result.gameObject.transform.IsChildOf(go_DialogueBar.transform))
                    {
                        clickedOnDialogueArea = true;
                        break;
                    }
                }

                if (!clickedOnDialogueArea)
                {
                    return;
                }
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
                    PointerEventData eventData = new PointerEventData(EventSystem.current)
                    {
                        position = Input.mousePosition
                    };
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

        if (isAutoMode)
        {
            isSkipping = false;
        }
    }
    public void SetAutoModeDelay(float newDelay)
    {

        autoModeDelay = Mathf.Lerp(0.1f, 1.0f, newDelay);
        PlayerPrefs.SetFloat("AutoModeDelay", autoModeDelay);
        PlayerPrefs.Save();
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

        if (_currentDialogueHandle.IsValid())
        {
            Addressables.Release(_currentDialogueHandle);
        }

        AsyncOperationHandle<DialogueData> newHandle = assetRef.LoadAssetAsync<DialogueData>();
        newHandle.Completed += handle =>
        {
            OnDialogueLoaded(handle, key);
        };
    }

    private void OnDialogueLoaded(AsyncOperationHandle<DialogueData> handle, string assetKey)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _currentDialogueHandle = handle;
            SetDialogueData(handle.Result, assetKey);
        }
        else
        {
            Debug.LogError($"Dialogue load failed from AssetReference: {assetKey}");
            Addressables.Release(handle);
        }
        isLoadingNextDialogue = false;
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

                if (!CheckCondition(line))
                {
                    dialogueIndex++;
                    continue;
                }

                if (string.IsNullOrEmpty(line.line))
                {
                    if (!string.IsNullOrEmpty(line.eventName))
                    {
                        if (line.eventName.Contains("FadeIn") || line.eventName.Contains("FadeOut"))
                        {
                            dialogueIndex++;
                            ProcessCommand(line.eventName);
                            return;
                        }

                        if (line.eventName.StartsWith("CMD:"))
                        {
                            ProcessCommand(line.eventName);
                        }
                        else
                        {
                            EventManager.Instance.TriggerEvent(line.eventName);
                        }
                    }

                    if (line.overrideNextDialogue != null && line.overrideNextDialogue.RuntimeKeyIsValid())
                    {
                        StartDialogueFromReference(line.overrideNextDialogue);
                        return;
                    }


                    dialogueIndex++;
                    continue;
                }

                SettingUI(true);
                ReadLogManager.Instance.MarkAsRead(_currentDialogueAssetKey, dialogueIndex);
                ApplyCharacterColors(line.speaker);

                if (line.speaker == "여주")
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

            if (dialogueIndex > 0)
            {
                Dialogue lastDialogueLine = currentDialogues[dialogueIndex - 1];
                if (lastDialogueLine.overrideNextDialogue != null && lastDialogueLine.overrideNextDialogue.RuntimeKeyIsValid())
                {
                    StartDialogueFromReference(lastDialogueLine.overrideNextDialogue);
                }
                else if (_currentDialogueData != null && _currentDialogueData.nextDialogueOnCompletion.RuntimeKeyIsValid())
                {
                    StartDialogueFromReference(_currentDialogueData.nextDialogueOnCompletion);
                }
                else
                {
                    EndDialogue();
                }
            }
            else
            {
                EndDialogue();
            }
        }
        finally
        {
            isProcessingDisplayNext = false;
        }
    }

    private string ProcessLinePlaceholders(string line)
    {
        string processedLine = line;
        if (processedLine.Contains("[여주"))
        {
            processedLine = processedLine.Replace("[여주는]", KoreanPostpositionHelper.Josa(playerFirstName, "이는/는"));
            processedLine = processedLine.Replace("[여주가]", KoreanPostpositionHelper.Josa(playerFirstName, "이가/가"));
            processedLine = processedLine.Replace("[여주를]", KoreanPostpositionHelper.Josa(playerFirstName, "이를/를"));
            processedLine = processedLine.Replace("[여주야]", KoreanPostpositionHelper.Josa(playerFirstName, "아/야"));
            processedLine = processedLine.Replace("[여주예요]", KoreanPostpositionHelper.Josa(playerFirstName, "이에요/예요"));
            processedLine = processedLine.Replace("[여주고]", KoreanPostpositionHelper.Josa(playerFirstName, "이고/고"));
            processedLine = processedLine.Replace("[여주도]", KoreanPostpositionHelper.Josa(playerFirstName, "이도/도"));
            processedLine = processedLine.Replace("[여주라고]", KoreanPostpositionHelper.Josa(playerFirstName, "이라고/라고"));
            processedLine = processedLine.Replace("[여주이]", KoreanPostpositionHelper.Josa(playerFirstName, "이/"));
            processedLine = processedLine.Replace("[여주]", playerFirstName);
            processedLine = processedLine.Replace("[여주이름이에요]", KoreanPostpositionHelper.Josa(playerLastName + playerFirstName, "이에요/예요"));
            processedLine = processedLine.Replace("[여주이름이고]", KoreanPostpositionHelper.Josa(playerLastName + playerFirstName, "이고/고"));
            processedLine = processedLine.Replace("[여주이름]", playerLastName + playerFirstName);
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
        if (isWaitingForChoiceClick)
        {
            if (choiceBlockerPanel != null) choiceBlockerPanel.SetActive(true);

            if (isAutoMode)
            {
                wasAutoModeActiveBeforeChoice = true;
                ToggleAuto(false);
            }
            ShowChoices(currentDialogues[dialogueIndex - 1].choices);
        }
        else
        {
            if (nextIndicator != null) nextIndicator.SetActive(true);
        }
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

        if (isWaitingForChoiceClick)
        {
            if (choiceBlockerPanel != null) choiceBlockerPanel.SetActive(true);

            if (isAutoMode)
            {
                wasAutoModeActiveBeforeChoice = true;
                ToggleAuto(false);
            }
            ShowChoices(currentDialogues[dialogueIndex - 1].choices);
        }
        else
        {
            if (nextIndicator != null)
            {
                nextIndicator.SetActive(true);
            }
        }
    }

    void ShowChoices(Choice[] choices)
    {
        foreach (Transform child in choicePanel)
        {
            ObjectPooler.Instance.ReturnToPool("ChoiceButton", child.gameObject);
        }

        if (nextIndicator != null) nextIndicator.SetActive(false);
        choicePanel.gameObject.SetActive(true);

        List<Choice> normalChoices = new List<Choice>();
        Choice allFailChoice = null;
        bool anyNormalChoiceIsActive = false;

        foreach (Choice choice in choices)
        {
            if (choice.conditionType == ConditionType.AllCharacterAffectionLess)
            {
                allFailChoice = choice;
            }
            else
            {
                normalChoices.Add(choice);
            }
        }

        foreach (Choice choice in normalChoices)
        {
            if (CheckChoiceCondition(choice) && CoinManager.Instance.PlayerCoin >= choice.requiredCoin)
            {
                anyNormalChoiceIsActive = true;
                break;
            }
        }

        List<Choice> choicesToShow = new List<Choice>();
        if (anyNormalChoiceIsActive)
        {
            choicesToShow.AddRange(normalChoices);
        }
        else
        {
            choicesToShow.AddRange(normalChoices);
            if (allFailChoice != null)
            {
                choicesToShow.Add(allFailChoice);
            }
        }

        foreach (Choice choice in choicesToShow)
        {
            GameObject buttonGO = ObjectPooler.Instance.SpawnFromPool("ChoiceButton", Vector3.zero, Quaternion.identity);
            if (buttonGO == null) continue;

            buttonGO.transform.SetParent(choicePanel, false);
            buttonGO.transform.localScale = Vector3.one;

            var button = buttonGO.GetComponent<Button>();
            var buttonText = buttonGO.GetComponentInChildren<TMP_Text>();
            string choiceText = choice.text;

            bool isInteractable = false;
            if (choice.conditionType == ConditionType.AllCharacterAffectionLess)
            {
                isInteractable = !anyNormalChoiceIsActive;
            }
            else
            {
                isInteractable = CheckChoiceCondition(choice) && CoinManager.Instance.PlayerCoin >= choice.requiredCoin;
            }
            button.interactable = isInteractable;

            if (!button.interactable && choice.conditionType != ConditionType.AllCharacterAffectionLess)
            {
                if (!CheckChoiceCondition(choice))
                {
                    choiceText += $" (호감도 부족, 상점에서 아이템을 구매하세요.)";
                }
                else
                {
                    choiceText += $" ({choice.requiredCoin} 코인 필요)";
                }
            }

            buttonText.text = choiceText;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnChoiceSelected(choice));
        }
    }

    void OnChoiceSelected(Choice choice)
    {
        if (isLoadingNextDialogue) return;

        isWaitingForChoiceClick = false;

        if (wasAutoModeActiveBeforeChoice)
        {
            ToggleAuto(true);
            wasAutoModeActiveBeforeChoice = false;
        }

        if (choice.requiredCoin > 0)
        {
            if (!CoinManager.Instance.SpendCoin(choice.requiredCoin))
            {
                return;
            }
        }

        if (choiceBlockerPanel != null) choiceBlockerPanel.SetActive(false);
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
            Debug.Log("대화 종료");
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
            case ConditionType.AllCharacterAffectionLess:
                if (string.IsNullOrEmpty(d.conditionTargetCharacter)) return true;
                string[] characters = d.conditionTargetCharacter.Split(',');
                foreach (string character in characters)
                {
                    if (AffectionManager.Instance.GetAffection(character.Trim()) >= d.conditionValue)
                    {
                        return false;
                    }
                }
                return true;
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
                return AffectionManager.Instance.GetAffection(choice.conditionTargetCharacter) > choice.conditionValue;
            case ConditionType.AffectionLess:
                return AffectionManager.Instance.GetAffection(choice.conditionTargetCharacter) < choice.conditionValue;
            case ConditionType.AffectionEqual:
                return AffectionManager.Instance.GetAffection(choice.conditionTargetCharacter) == choice.conditionValue;
            case ConditionType.AllCharacterAffectionLess:
                if (string.IsNullOrEmpty(choice.conditionTargetCharacter)) return true;
                string[] characters = choice.conditionTargetCharacter.Split(',');
                foreach (string character in characters)
                {
                    if (AffectionManager.Instance.GetAffection(character.Trim()) >= choice.conditionValue)
                    {
                        return false;
                    }
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

    public void SetTypingSpeed(float sliderValue)
    {
        typingSpeed = Mathf.Lerp(0.1f, 0.01f, sliderValue);

        PlayerPrefs.SetFloat("TypingSpeed", typingSpeed);
        PlayerPrefs.Save();
    }

    private void ProcessCommand(string command)
    {
        string[] parts = command.Split(':');
        if (parts.Length < 2)
        {
            Debug.LogWarning($"잘못된 명령어 형식: {command}");
            return;
        }

        string commandType = parts[1];

        if (commandType.StartsWith("Fade"))
        {
            if (ScreenFadeManager.Instance == null)
            {
                Debug.LogWarning("ScreenFadeManager를 찾을 수 없습니다.");
                return;
            }

            float duration = 1.0f;
            if (parts.Length > 2 && float.TryParse(parts[2], out float parsedDuration))
            {
                duration = parsedDuration;
            }

            Action onStartCallback = () => SettingUI(false);
            Action onCompleteCallback = () => DisplayNext();

            switch (commandType)
            {
                case "FadeOut":
                    ScreenFadeManager.Instance.FadeOut(duration, onStartCallback, onCompleteCallback);
                    return;

                case "FadeIn":
                    ScreenFadeManager.Instance.FadeIn(duration, onStartCallback, onCompleteCallback);
                    return;

                case "FadeInOut":
                    ScreenFadeManager.Instance.FadeInOut(onStart: onStartCallback, onComplete: onCompleteCallback);
                    return;
            }
        }



        switch (commandType)
        {
            case "SetFlag":
                if (parts.Length > 3)
                {
                    string flagKey = parts[2];
                    if (bool.TryParse(parts[3], out bool flagValue))
                    {
                        FlagManager.Instance.SetFlag(flagKey, flagValue);
                        Debug.Log($"플래그 설정: {flagKey} = {flagValue}");
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

            case "SaveProgress":
                Debug.Log("중간 에피소드 종료: 1번 슬롯에 진행 상황을 자동 저장합니다.");
                if (SaveLoadManager.Instance != null)
                {
                    SaveLoadManager.Instance.SaveGame(0);
                }
                break;

            case "CompletePlaythrough":
                Debug.Log("최종 에피소드 종료: 1회차 완료 처리 및 최종 저장을 시도합니다.");
                if (GameProgressionManager.Instance != null)
                {
                    GameProgressionManager.Instance.MarkFirstPlaythroughComplete();
                }
                if (SaveLoadManager.Instance != null)
                {
                    SaveLoadManager.Instance.SaveGame(0);
                }
                break;

            default:
                Debug.LogWarning($"알 수 없는 명령어입니다: {commandType}");
                break;
        }
    }
    public void SetDialogueUIVisibility(bool isVisible)
    {
        if (instantiatedDialogueUI != null)
        {
            instantiatedDialogueUI.SetActive(isVisible);
        }
    }
}