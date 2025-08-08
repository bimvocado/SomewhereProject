using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Heart : MonoBehaviour
{
    public Image[] HeartUI;
    private int heartNum;
    public GameObject GameOverUI;
    
    
    public void turnOffHeart()
    {
        HeartUI[heartNum].color = Color.black;
        heartNum++;
        
        if (heartNum == 2)
            GameOver();
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        GameOverUI.SetActive(true);
    }
}