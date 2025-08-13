using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject PauseUI; //일시정지 UI
    public bool isPause; //현재 일시정지 활성화 여부
    
    void Start()
    {
        StopPause();
    }
    
    void Update()
    {
        //ESC로 일시정지 진입
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause == false)
            {
                StartPause();
            }
            else
            {
                StopPause();
            }
        }
    }

    public void StartPause() 
    {
        Time.timeScale = 0; //시간에 따른 변화량이 0 -> 게임이 진행되어도 변화량 x
        isPause = true;
        PauseUI.SetActive(true); 
    }

    public void StopPause()
    {
        Time.timeScale = 1; //시간의 변화량이 1이다. -> 정상적인 속도
        isPause = false;
        PauseUI.SetActive(false); 
    }
}