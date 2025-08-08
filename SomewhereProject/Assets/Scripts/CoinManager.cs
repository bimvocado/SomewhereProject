using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    public int PlayerCoin { get; private set; }

    private const string CoinSaveKey = "PlayerCoin";

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


        PlayerCoin = 0;
    }

    public void AddCoin(int amount)
    {
        if (amount <= 0) return;
        PlayerCoin += amount;
        Debug.Log($"코인 {amount} 획득. 현재 코인: {PlayerCoin}");
    }

    public bool SpendCoin(int amount)
    {
        if (amount <= 0) return false;

        if (PlayerCoin >= amount)
        {
            PlayerCoin -= amount;
            Debug.Log($"코인 {amount} 사용. 현재 코인: {PlayerCoin}");
            return true;
        }

        Debug.Log("코인이 부족합니다.");
        return false;
    }


    public void LoadCoin(int amount)
    {
        PlayerCoin = amount;
    }
}