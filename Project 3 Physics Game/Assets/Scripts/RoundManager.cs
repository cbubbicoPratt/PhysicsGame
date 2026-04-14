using System;
using TMPro;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    //we want rounds for each randomized set of pegs
    //score requirement = current round * 64 + (excess points from last round / 2)
    private int currentRound = 1;
    private int scoreRequirement;
    private int excessScore = 0;

    public TextMeshProUGUI roundDisplay;
    public static event Action onUpdate;

    private void OnEnable()
    {
        PhysicsObject.OnEnd += UpdateRound;
    }

    private void Update()
    {
        scoreRequirement = currentRound * 256 + (excessScore / 2);
        roundDisplay.text = "Round: " + currentRound;
    }

    public void UpdateRound()
    {
        if (ScoreManager.GetScore() >= scoreRequirement)
        {
            currentRound++;
            excessScore = ScoreManager.GetScore() - scoreRequirement;
            onUpdate?.Invoke();
        }
    }

    
    public int GetCurrentRound() { return currentRound; }
    //called by ScoreManager to display the needed score
    public int GetScoreReq() { return scoreRequirement; }
    //called by LifeManager to calculate extra lives given
    public int GetExcessScore() { return excessScore; }
}
