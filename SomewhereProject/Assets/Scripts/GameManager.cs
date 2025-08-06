using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        NameInputManager.Instance.ShowNameInputPanel(true);
    }
}