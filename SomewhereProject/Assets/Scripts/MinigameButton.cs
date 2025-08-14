using UnityEngine;
using System.Collections.Generic;

public class MinigameButton : MonoBehaviour
{
    [Header("시작할 미니게임 씬 이름")]
    [Tooltip("미니게임 시작 시 가장 먼저 불러올 씬의 이름 (예: 설명 씬)")]
    public string startingMinigameScene;

    public void LaunchMinigame()
    {
        if (string.IsNullOrEmpty(startingMinigameScene))
        {
            Debug.LogError("MinigameButton에 시작할 씬 이름이 지정되지 않았습니다!");
            return;
        }

        if (MinigameManager.Instance != null)
        {
            MinigameManager.Instance.StartMinigame(new List<string> { startingMinigameScene });
        }
        else
        {
            Debug.LogError("MinigameManager 인스턴스를 찾을 수 없습니다!");
        }
    }
}