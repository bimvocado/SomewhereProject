using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreCount : MonoBehaviour
{
    public TMP_Text ScoreText;
    public TMP_Text CoinText;
    public int Score;
    public int Coin;
    

    void Update()
    {
        if (Score % 10 == 0)
            Coin = Score % 10;
        
        ScoreText.text = "Score: " + Score;
        CoinText.text = "Coin: " + Coin;
    }
    
}