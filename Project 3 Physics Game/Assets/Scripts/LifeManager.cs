using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeManager : MonoBehaviour
{
    public int currentLives = 3;
    public int maxLives = 10;

    private RoundManager roundManager;
    public TextMeshProUGUI livesDisplay;


    private void Awake()
    {
        roundManager = Object.FindFirstObjectByType<RoundManager>();
        livesDisplay.text = "Lives: " + currentLives;
    }
    private void OnEnable()
    {
        PhysicsObject.OnEnd += UpdateLives;
    }

    public void UpdateLives()
    {
        if (roundManager == null) return;
        if (roundManager.GetScoreReq() > ScoreManager.GetScore())
        {
            currentLives--;
        }
        else
        {
            currentLives += (roundManager.GetExcessScore() / 128);
            if (currentLives > maxLives) currentLives = maxLives;
        }
        if(currentLives <= 0)  SceneManager.LoadScene("GameOver");
        livesDisplay.text = "Lives: " + currentLives;
    }
}
