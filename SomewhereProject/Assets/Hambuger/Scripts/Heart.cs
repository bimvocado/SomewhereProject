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

    private void Start()
    {
        GameOverUI.SetActive(false);
    }

    public void turnOffHeart()
    {
        HeartUI[heartNum].color = Color.black;
        heartNum++;

        if (heartNum == 3)
        {
            StartCoroutine("waitGameOver");
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        GameOverUI.SetActive(true);
    }

    IEnumerator waitGameOver()
    {
        yield return new WaitForSeconds(1.5f);
        GameOver();
    }
}