using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

public enum ConditionType { None, FlagTrue, FlagFalse, AffectionGreater, AffectionLess, AffectionEqual,
    FlagTrueAndAffectionGreater, FlagTrueAndAffectionLess, FlagFalseAndAffectionGreater, FlagFalseAndAffectionLess,
    AllCharacterAffectionLess }


[System.Serializable]
public class Dialogue
{
    public string speaker;
    [TextArea]
    public string line;

    public string eventName;
    public ConditionType conditionType;
    public string conditionKey;

    public string conditionTargetCharacter;
    public int conditionValue;

    public bool hasChoices;
    public Choice[] choices;

    public AssetReferenceT<DialogueData> overrideNextDialogue;
}