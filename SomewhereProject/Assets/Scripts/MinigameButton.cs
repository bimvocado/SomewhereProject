using UnityEngine;
using System.Collections.Generic;

public class MinigameButton : MonoBehaviour
{
    [Header("������ �̴ϰ��� �� �̸�")]
    [Tooltip("�̴ϰ��� ���� �� ���� ���� �ҷ��� ���� �̸� (��: ���� ��)")]
    public string startingMinigameScene;

    public void LaunchMinigame()
    {
        if (string.IsNullOrEmpty(startingMinigameScene))
        {
            Debug.LogError("MinigameButton�� ������ �� �̸��� �������� �ʾҽ��ϴ�!");
            return;
        }

        if (MinigameManager.Instance != null)
        {
            MinigameManager.Instance.StartMinigame(new List<string> { startingMinigameScene });
        }
        else
        {
            Debug.LogError("MinigameManager �ν��Ͻ��� ã�� �� �����ϴ�!");
        }
    }
}