using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class InitialAffection
{
    public string characterName;
    public int initialValue;
}

public class AffectionManager : MonoBehaviour
{
    public static AffectionManager Instance { get; private set; }

    [Header("초기 호감도 설정")]
    [SerializeField] private List<InitialAffection> initialAffections;

    private Dictionary<string, int> affections = new Dictionary<string, int>();
    private int defaultAffection = 0;

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

    private void Start()
    {
        InitializeAffections();

        if (EventManager.Instance != null)
        {
            EventManager.Instance.StartListening("ResetAffection", ResetAllAffections);
        }
    }

    private void InitializeAffections()
    {
        foreach (var affectionSetting in initialAffections)
        {
            if (!string.IsNullOrEmpty(affectionSetting.characterName))
            {
                affections[affectionSetting.characterName] = affectionSetting.initialValue;
                Debug.Log($"초기 호감도 설정: {affectionSetting.characterName} = {affectionSetting.initialValue}");
            }
        }
    }

    public void ChangeAffection(string characterName, int amount)
    {
        if (string.IsNullOrEmpty(characterName) || amount == 0) return;

        if (!affections.ContainsKey(characterName))
        {
            affections[characterName] = defaultAffection;
        }
        affections[characterName] += amount;
        Debug.Log($"캐릭터 '{characterName}'의 호감도 {amount} 변경, 현재 호감도: {affections[characterName]}");

        if (EventManager.Instance != null)
        {
            EventManager.Instance.TriggerEvent($"AffectionChanged_{characterName}");
        }
    }

    public void ResetAllAffections()
    {
        affections = new Dictionary<string, int>();
        InitializeAffections();
    }

    public void ResetAffection(string characterName)
    {
        if (string.IsNullOrEmpty(characterName)) return;

        if (affections.ContainsKey(characterName))
        {
            affections.Remove(characterName);
        }

        var initialSetting = initialAffections.FirstOrDefault(x => x.characterName == characterName);
        if (initialSetting != null)
        {
            affections[characterName] = initialSetting.initialValue;
        }


        if (EventManager.Instance != null)
        {
            EventManager.Instance.TriggerEvent($"AffectionChanged_{characterName}");
        }
    }

    public int GetAffection(string characterName)
    {
        if (affections.ContainsKey(characterName))
        {
            return affections[characterName];
        }
        return defaultAffection;
    }

    public List<string> GetCharactersWithHighestAffection()
    {
        if (affections == null || affections.Count == 0)
        {
            return new List<string>();
        }

        int maxAffection = affections.Values.Max();

        List<string> winners = affections.Where(pair => pair.Value == maxAffection)
                                         .Select(pair => pair.Key)
                                         .ToList();
        return winners;
    }

    public Dictionary<string, int> GetAffections()
    {
        return new Dictionary<string, int>(affections);
    }

    public void LoadAffections(Dictionary<string, int> data)
    {
        affections = new Dictionary<string, int>(data);
        Debug.Log("저장된 호감도 데이터를 불러옴.");
    }
}