using UnityEngine;
using TMPro;
public class CoinDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text coinText;

    private void OnEnable()
    {
        CoinManager.OnCoinChanged += UpdateCoinText;
    }

    private void OnDisable()
    {
        CoinManager.OnCoinChanged -= UpdateCoinText;
    }

    private void UpdateCoinText(int amount)
    {
        if (coinText != null)
        {
            coinText.text = amount.ToString();
        }
    }
}