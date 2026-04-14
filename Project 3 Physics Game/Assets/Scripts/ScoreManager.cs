using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //empty object to store score
    //function to update
    public TextMeshProUGUI scoreText;
    public static int score = 0;
    private RoundManager roundManager;

    private void OnEnable()
    {
        RoundManager.onUpdate += ResetScore;
    }
    private void Awake()
    {
        roundManager = Object.FindFirstObjectByType<RoundManager>();
    }

    private void Update()
    {
        scoreText.text = "Score: " + score + "/" + roundManager.GetScoreReq();
    }

    public static void UpdateScore(int addScore)
    {
        score += addScore;
    }

    public static int GetScore() { return score; }

    public static void ResetScore(){ score = 0; }
}
