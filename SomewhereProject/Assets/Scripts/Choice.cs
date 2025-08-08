using UnityEngine.AddressableAssets;

[System.Serializable]
public class Choice
{
    public string text;
    public AssetReferenceT<DialogueData> nextDialogue;
    public int affectionChange;
    public string targetCharacterForAffection;
    public bool resetsAffection = false;

    public int requiredCoin;
    public string flagToSet;
    public bool flagValue;

    public ConditionType conditionType;
    public string conditionKey;
    public string conditionTargetCharacter;
    public int conditionValue;
}