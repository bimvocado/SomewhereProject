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

    private void Start()
    {
        SetText();
    }


    public void SetText()
    {
        ScoreText.text = "Score: " + Score;
        CoinText.text = "Coin: " + Coin;
    }

    public void GetScore()
    {
        Score += 1;
        Coin = Score / 10;
        SetText();
    }
    
}