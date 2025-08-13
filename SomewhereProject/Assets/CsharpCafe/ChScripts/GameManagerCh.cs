using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerCh : MonoBehaviour
{
    public static GameManagerCh instance;

    public GameObject backgroundPanel;
    public GameObject victoryPanel;
    public GameObject losePanel;

    public int goal;
    public int moves;
    public int points;

    public bool isGameEnded;

    private void Awake()
    {
        instance = this;
    }

    public void Initialized(int _moves, int _goal)
    {
        moves = _moves;
        goal = _goal;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ProcessTurn(int _pointsToGain, bool _subtractMoves)
    {
        points += _pointsToGain;
        if (_subtractMoves)
        {
            moves--;
        }
        if (points >= goal)
        {
            isGameEnded = true;

            backgroundPanel.SetActive(true);
            victoryPanel.SetActive(true);
            return;
        }
        if (moves == 0)
        {
            isGameEnded = true;
            backgroundPanel.SetActive(true);
            losePanel.SetActive(true);
            return;
        }
    }

    public void WinGame()
    {
        SceneManager.LoadScene(0);
    }

    public void LoseGame()
    {
        SceneManager.LoadScene(0);
    }
}
