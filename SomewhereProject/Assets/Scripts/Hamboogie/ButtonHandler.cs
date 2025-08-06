using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;


public enum ButtonType
{
    Start,
    Exit,
    Continue,
    How
}

public class ButtonHandler : MonoBehaviour
{
    public ButtonType currentType;

    public void onButtonClick()
    {
        switch (currentType)
        {
            case ButtonType.Start:
                SceneManager.LoadScene("HambugerGame");
                break;
            
            case ButtonType.Continue:
                break;
                
            case ButtonType.Exit:
                SceneManager.LoadScene("HambbugerMenu");
                break;
            
            case ButtonType.How:
                SceneManager.LoadScene("HambugerHow");
                break;
        }
    }
}