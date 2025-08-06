using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public Dialogue[] dialogues;
    public AssetReferenceT<DialogueData> nextDialogueOnCompletion;
}
