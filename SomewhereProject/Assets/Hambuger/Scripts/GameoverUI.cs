using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameoverUI : MonoBehaviour
{
    public TMP_Text FinalScore;
    public TMP_Text FinalCoin;
    

    void OnEnable()
    {
        FinalScore.text = ScoreCount.instance.GetFinalScore();
        FinalCoin.text = ScoreCount.instance.GetFinalCoin();

    }
}
