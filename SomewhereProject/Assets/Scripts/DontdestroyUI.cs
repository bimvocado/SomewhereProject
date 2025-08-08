using NUnit.Framework.Internal.Execution;
using UnityEngine;

public class DontdestroyUI : MonoBehaviour
{
    public static DontdestroyUI Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }
}
