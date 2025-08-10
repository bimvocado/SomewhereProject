using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlagManager : MonoBehaviour
{
    public static FlagManager Instance;
    public static event UnityAction<string> OnFlagChanged;
    private Dictionary<string, bool> flags = new Dictionary<string, bool>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetFlag(string key, bool value)
    {
        flags[key] = value;
        OnFlagChanged?.Invoke(key);
    }

    public bool GetFlag(string key)
    {
        if (string.IsNullOrEmpty(key)) return false;
        return flags.ContainsKey(key) && flags[key];
    }

    public Dictionary<string, bool> GetFlags()
    {
        return new Dictionary<string, bool>(flags);
    }

    public void LoadFlags(Dictionary<string, bool> data)
    {
        flags = new Dictionary<string, bool>(data);
    }
}
