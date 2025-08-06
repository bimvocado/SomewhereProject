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
        Debug.Log($"���� {amount} ȹ��. ���� ����: {PlayerCoin}");
    }

    public bool SpendCoin(int amount)
    {
        if (amount <= 0) return false;

        if (PlayerCoin >= amount)
        {
            PlayerCoin -= amount;
            Debug.Log($"���� {amount} ���. ���� ����: {PlayerCoin}");
            return true;
        }

        Debug.Log("������ �����մϴ�.");
        return false;
    }


    public void LoadCoin(int amount)
    {
        PlayerCoin = amount;
    }
}