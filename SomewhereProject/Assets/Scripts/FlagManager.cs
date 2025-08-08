using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    public static FlagManager Instance;

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
