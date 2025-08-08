using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreCount : MonoBehaviour
{
    public static ScoreCount instance;
    
    public TMP_Text ScoreText;
    public TMP_Text CoinText;

    public int Score;
    public int Coin;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetText();
    }


    public void SetText()
    {
        ScoreText.text = "Score: " + Score;
        CoinText.text = "Coin: " + Coin;
    }

    public string GetFinalScore()
    {
        return ScoreText.text;
    }

    public string GetFinalCoin()
    {
        return CoinText.text;
    }
    
    public void GetScore()
    {
        Score += 1;
        Coin = Score / 10;
        SetText();
    }
    
}