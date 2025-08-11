using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System.Linq;
public class AffectionDialogueStarter : MonoBehaviour
{
    [System.Serializable]
    public class CharacterRoute
    {
        public string characterName;
        public AssetReferenceT<DialogueData> dialogueRef;
    }

    [Header("캐릭터별 스토리 루트")]
    public List<CharacterRoute> characterRoutes;

    public void StartHighestAffectionRoute()
    {
        if (characterRoutes == null || characterRoutes.Count == 0)
        {
            return;
        }

        if (AffectionManager.Instance == null)
        {
            return;
        }

        List<string> routableCharacters = characterRoutes.Select(route => route.characterName).ToList();

        int maxAffection = -1;
        List<string> winners = new List<string>();

        foreach (string characterName in routableCharacters)
        {
            int currentAffection = AffectionManager.Instance.GetAffection(characterName);

            if (currentAffection > maxAffection)
            {
                maxAffection = currentAffection;
                winners.Clear();
                winners.Add(characterName);
            }
            else if (currentAffection == maxAffection)
            {
                winners.Add(characterName);
            }
        }

        if (winners.Count == 0)
        {
            return;
        }

        string finalWinner;
        if (winners.Count == 1)
        {
            finalWinner = winners[0];
        }
        else
        {
            int randomIndex = Random.Range(0, winners.Count);
            finalWinner = winners[randomIndex];
        }

        CharacterRoute winnerRoute = characterRoutes.FirstOrDefault(route => route.characterName == finalWinner);

        if (winnerRoute != null && winnerRoute.dialogueRef != null && winnerRoute.dialogueRef.RuntimeKeyIsValid())
        {
            DialogueManager.Instance.StartDialogueFromReference(winnerRoute.dialogueRef);
        }
    }
}