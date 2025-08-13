using UnityEngine;
using System.Collections.Generic; // List를 사용하기 위해 추가

public class MinigameButton : MonoBehaviour
{
    [Header("불러올 미니게임 씬 목록")]
    public List<string> minigameSceneNames;

    public void LaunchMinigame()
    {
        if (minigameSceneNames == null || minigameSceneNames.Count == 0)
        {
            Debug.LogError("MinigameButton에 미니게임 씬 목록이 지정되지 않았습니다!");
            return;
        }

        if (MinigameManager.Instance != null)
        {
            MinigameManager.Instance.StartMinigame(minigameSceneNames);
        }
        else
        {
            Debug.LogError("MinigameManager 인스턴스를 찾을 수 없습니다!");
        }
    }
}