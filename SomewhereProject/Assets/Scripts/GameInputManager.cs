using UnityEngine;

public class GameInputManager : MonoBehaviour
{
    void Start()
    {
        NameInputManager.Instance.ShowNameInputPanel(true);
    }
}