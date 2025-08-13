using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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

    public TMP_Text Points;
    public TMP_Text Goals;
    public TMP_Text Moves;

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
        Initialized(moves, goal);
        points = 0;
    }

    void Update()
    {
        Points.text = "Points: " + points.ToString();
        Moves.text = "Moves: " + moves.ToString();
        Goals.text = "Goals: " + goal.ToString();
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
            PotionBoard.Instance.potionParent.SetActive(false);
            return;
        }
        if (moves == 0)
        {
            isGameEnded = true;
            backgroundPanel.SetActive(true);
            losePanel.SetActive(true);
            PotionBoard.Instance.potionParent.SetActive(false);
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
