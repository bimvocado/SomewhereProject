using UnityEngine;
using System;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    public static event Action<int> OnCoinChanged;

    public int PlayerCoin { get; private set; }
    private const string CoinSaveKey = "GlobalPlayerCoin";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        PlayerCoin = PlayerPrefs.GetInt(CoinSaveKey, 0);
    }

    private void Start()
    {
        OnCoinChanged?.Invoke(PlayerCoin);
    }

    public void AddCoin(int amount)
    {
        if (amount <= 0) return;
        PlayerCoin += amount;
        SaveCoin();
        OnCoinChanged?.Invoke(PlayerCoin);
        Debug.Log($"코인 {amount} 획득. 현재 코인: {PlayerCoin}");
    }

    public bool SpendCoin(int amount)
    {
        if (amount < 0) return false;

        if (PlayerCoin >= amount)
        {
            PlayerCoin -= amount;
            SaveCoin();
            OnCoinChanged?.Invoke(PlayerCoin);
            Debug.Log($"코인 {amount} 사용. 현재 코인: {PlayerCoin}");
            return true;
        }

        Debug.Log("코인이 부족합니다.");
        return false;
    }

    private void SaveCoin()
    {
        PlayerPrefs.SetInt(CoinSaveKey, PlayerCoin);
        PlayerPrefs.Save();
    }
}