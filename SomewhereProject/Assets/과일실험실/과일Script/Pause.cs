using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.Animations;
using Unity.VisualScripting;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button resumeButton;
    public Button retryButton;
    public Button quitButton;
    //public Button pauseButton;
    private GameManagerFr gameManager;

    private bool isPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenuUI.SetActive(false);
        resumeButton.onClick.AddListener(Resume);
        retryButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
        //pauseButton.onClick.AddListener(ShowPause);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }
    public void ShowPause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void Pause() { 
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        gameManager.NewGame();
        pauseMenuUI.SetActive(false);
    }
    public void QuitGame()
    {
        //quit logic-where to go after quit? 
    }
}
