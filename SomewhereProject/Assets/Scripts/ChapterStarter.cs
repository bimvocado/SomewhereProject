using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections;

public class ChapterStarter : MonoBehaviour
{
    public AssetReferenceT<DialogueData> startingDialogueRef;

    void Start()
    {
        if (startingDialogueRef != null && startingDialogueRef.RuntimeKeyIsValid() && DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogueFromReference(startingDialogueRef);
        }
    }

}