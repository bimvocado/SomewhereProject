using UnityEngine;
using System.Collections.Generic; // List�� ����ϱ� ���� �߰�

public class MinigameButton : MonoBehaviour
{
    [Header("�ҷ��� �̴ϰ��� �� ���")]
    public List<string> minigameSceneNames;

    public void LaunchMinigame()
    {
        if (minigameSceneNames == null || minigameSceneNames.Count == 0)
        {
            Debug.LogError("MinigameButton�� �̴ϰ��� �� ����� �������� �ʾҽ��ϴ�!");
            return;
        }

        if (MinigameManager.Instance != null)
        {
            MinigameManager.Instance.StartMinigame(minigameSceneNames);
        }
        else
        {
            Debug.LogError("MinigameManager �ν��Ͻ��� ã�� �� �����ϴ�!");
        }
    }
}