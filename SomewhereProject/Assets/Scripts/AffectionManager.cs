using System.Collections.Generic;
using UnityEngine;
using System.Linq; // LINQ를 위해 필요합니다.

public class AffectionManager : MonoBehaviour
{
    public static AffectionManager Instance { get; private set; }

    private Dictionary<string, int> affections = new Dictionary<string, int>();
    private int defaultAffection = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log($"[AffectionManager] Awake 호출됨. Instance 설정 완료: {gameObject.name}");
        }
        else
        {
            Debug.LogWarning($"[AffectionManager] 중복된 인스턴스 발견: {gameObject.name} 파괴됨.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.StartListening("ResetAffection", ResetAllAffections);
            Debug.Log($"[AffectionManager] Start 호출됨. EventManager에 구독 완료.");
        }
        else
        {
            Debug.LogError("[AffectionManager] Start()에서 EventManager.Instance를 찾을 수 없습니다! EventManager가 올바르게 초기화되지 않았거나 스크립트 실행 순서에 문제가 있을 수 있습니다.");
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
    }

    public void ResetAllAffections()
    {
        affections = new Dictionary<string, int>();
        Debug.Log("모든 캐릭터의 호감도가 초기화되었습니다.");
    }

    public void ResetAffection(string characterName)
    {
        if (string.IsNullOrEmpty(characterName)) return;

        if (affections.ContainsKey(characterName))
        {
            affections.Remove(characterName);
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
    }
}