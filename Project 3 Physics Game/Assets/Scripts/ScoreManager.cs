using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //empty object to store score
    //function to update
    public TextMeshProUGUI scoreText;
    public static int score = 0;

    private void Update()
    {
        scoreText.text = "Score: " + score;
    }

    public static void UpdateScore(int addScore)
    {
        score += addScore;
    }
}
