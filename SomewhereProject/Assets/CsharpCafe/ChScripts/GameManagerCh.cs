using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class GameManagerCh : MonoBehaviour
{
    public static GameManagerCh instance;

    public GameObject backgroundPanel;
    public GameObject victoryPanel;
    public GameObject losePanel;

    public int goal;
    public int moves;
    public int points;
    public int coins;

    public bool isGameEnded;

    public TMP_Text Points;
    public TMP_Text Goals;
    public TMP_Text Moves;
    public TMP_Text Coins;

    public TMP_Text VictoryPoints;
    public TMP_Text VictoryCoins;

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
        coins = 0;
    }

    private void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        Points.text = "Points: " + points;
        Coins.text = "Coins: " + (points/10);
        Moves.text = "Moves: " + moves;
        Goals.text = "Goal: " + goal;
    }

    public void ProcessTurn(int _pointsToGain, bool _subtractMoves)
    {
        points += _pointsToGain;
        if (_subtractMoves)
        {
            moves--;
        }

        UpdateUI();

        if (points >= goal)
        {
            isGameEnded = true;

            backgroundPanel.SetActive(true);
            victoryPanel.SetActive(true);
            PotionBoard.Instance.potionParent.SetActive(false);

            VictoryPoints.text = "Points: " + points;
            VictoryCoins.text = "Coins: " + (points / 10);

            coins = points / 10;
            if (CoinManager.Instance != null)
            {
                CoinManager.Instance.AddCoin(coins);
            }

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
