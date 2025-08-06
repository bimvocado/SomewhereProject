using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections;

public class ChapterStarter : MonoBehaviour
{
    public AssetReferenceT<DialogueData> startingDialogueRef;

    void Start()
    {
        StartCoroutine(StartChapterSequence());
    }

    private IEnumerator StartChapterSequence()
    {
        yield return FadeManager.Instance.FadeIn();

        if (startingDialogueRef != null && startingDialogueRef.RuntimeKeyIsValid() && DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogueFromReference(startingDialogueRef);
        }
        else
        {
            Debug.LogWarning("다이얼로그 연결 안 됨");
        }
    }
}