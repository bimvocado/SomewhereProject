using UnityEngine;

public class MinigameReturnButton : MonoBehaviour
{
    public void ReturnToMainGame()
    {
        if (MinigameManager.Instance != null)
        {
            MinigameManager.Instance.EndMinigame();
        }
    }
}