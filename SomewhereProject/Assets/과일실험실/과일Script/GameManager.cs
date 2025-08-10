












using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI FinalScore;
    public TextMeshProUGUI ScoreInfo;
    public Button StartButton;
    public Image GameOverImage;
    public Image GameStartImage;
    public Button BackButton;
    public Button RetryButton;

    public Button pauseButton;
    public GameObject pauseMenuUI;
    public Button resumeButton;
    public Button inPauseRetryButton;
    public Button quitButton;

    private bool isPaused = false;

    private Blade blade;
    private Spawner spawner;
    //private ParticleSystem bombParticle;
    
    private int score;

    private int Coin = 0;

    public float threshold;
    public float elapsedTime1;
    public TextMeshProUGUI TimeLeft;
    private bool gameStarted = false;

    private void Awake()
    {
        blade = FindAnyObjectByType<Blade>();
        spawner = FindAnyObjectByType<Spawner>();
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        pauseButton.gameObject.SetActive(false);
        GameStartImage.color = Color.white;
        GameOverImage.color = Color.clear;
        FinalScore.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        RetryButton.gameObject.SetActive(false);

        pauseMenuUI.SetActive(false);
        resumeButton.onClick.AddListener(Resume);
        inPauseRetryButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
        pauseButton.onClick.AddListener(ShowPause);

        StartButton.onClick.RemoveAllListeners();
        StartButton.onClick.AddListener(NewGame);
        elapsedTime1 = 0f;
        TimeLeft.gameObject.SetActive(false);
    }
    public void NewGame()
    {
        TimeLeft.gameObject.SetActive(true);
        elapsedTime1 = 0f;
        gameStarted = true;
        ScoreInfo.gameObject.SetActive(false);
        StopCoroutine(ExplodeSequence());
        pauseButton.gameObject.SetActive(true); 
        FinalScore.gameObject.SetActive(false);
        GameOverImage.color = Color.clear;
        RetryButton.gameObject.SetActive(false);
        spawner.StartSpawning();
        GameStartImage.color = Color.clear;
        StartButton.gameObject.SetActive(false);
        BackButton.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        Time.timeScale = 1f;
        blade.enabled = true;
        spawner.enabled = true;
        score = 0;
        scoreText.text = score.ToString();

        ClearScene();
    }

    private void ClearScene()
    {
        Fruit[] fruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
        foreach (Fruit f in fruits)
        {
            Destroy(f.gameObject);
        }
        Bomb[] bombs = FindObjectsByType<Bomb>(FindObjectsSortMode.None);
        foreach (Bomb b in bombs)
        {
            Destroy(b.gameObject);
        }
        elapsedTime1 = 0f;
    }
    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
    public void Explode()
    {
        blade.enabled = false;
        spawner.enabled = false;
        gameStarted = false;
        
        StartCoroutine(ExplodeSequence());
    }
    private IEnumerator ExplodeSequence()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1f);
        GameOverImage.color = Color.white;
        spawner.StopSpawning();
        if (score < 30)
            Coin = 0;
        else if (score >= 30 && score < 60)
            Coin = 1;
        else if (score < 90)
            Coin = 2;
        else if (score < 120)
            Coin = 3;
        else if (score < 150)
            Coin = 4;
        else if (score < 180)
            Coin = 5;
        else if (score < 210)
            Coin = 6;
        else if (score < 240)
            Coin = 7;
        else if (score < 270)
            Coin = 8;
        else if (score < 300)
            Coin = 9;
        RetryButton.gameObject.SetActive(true);
        BackButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        FinalScore.text = "Score : "+score.ToString()+"\nCollected Coin : "+Coin.ToString();
        FinalScore.gameObject.SetActive(true);
        RetryButton.onClick.RemoveAllListeners();  
        RetryButton.onClick.AddListener(NewGame);
        yield return new WaitForSecondsRealtime(3f);
    }
    public void ShowPause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void Pause()
    {
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
        NewGame();
        pauseMenuUI.SetActive(false);
    }
    public void QuitGame()
    {
        //quit logic-where to go after quit? 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
        if (gameStarted == true)
        {
            float timeleft = threshold - elapsedTime1;
            TimeLeft.text = timeleft.ToString("#.00");
            elapsedTime1 += Time.deltaTime;
            if (elapsedTime1 >= threshold)
            {
                Explode();
            }
        }

    }
}
