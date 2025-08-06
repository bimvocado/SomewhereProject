using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;

public class AffectionDialogueStarter : MonoBehaviour
{
    [System.Serializable]
    public class CharacterRoute
    {
        public string characterName;
        public AssetReferenceT<DialogueData> dialogueRef;
    }

    [Header("ĳ���ͺ� ���丮 ��Ʈ")]
    public List<CharacterRoute> characterRoutes;

    private Dictionary<string, AssetReferenceT<DialogueData>> routeDictionary;

    private void Awake()
    {
        routeDictionary = new Dictionary<string, AssetReferenceT<DialogueData>>();
        foreach (var route in characterRoutes)
        {
            routeDictionary[route.characterName] = route.dialogueRef;
        }
    }

    public void StartHighestAffectionRoute()
    {
        List<string> topCharacters = AffectionManager.Instance.GetCharactersWithHighestAffection();
        string winner = "";

        if (topCharacters.Count == 1)
        {
            winner = topCharacters[0];
        }
        else if (topCharacters.Count > 1)
        {
            int randomIndex = Random.Range(0, topCharacters.Count);
            winner = topCharacters[randomIndex];
        }

        if (!string.IsNullOrEmpty(winner) && routeDictionary.TryGetValue(winner, out var routeDialogueRef))
        {
            DialogueManager.Instance.StartDialogueFromReference(routeDialogueRef);
            return;
        }
    }
}