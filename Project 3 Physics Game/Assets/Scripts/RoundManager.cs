using System;
using TMPro;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    //we want rounds for each randomized set of pegs
    //score requirement = current round * 64 + (excess points from last round / 2)
    private int currentRound = 1;
    private int excessScore = 0;
    private int scoreRequirement = 256;

    public TextMeshProUGUI roundDisplay;
    public static event Action onUpdate;

    private void OnEnable()
    {
        PhysicsObject.OnEnd += UpdateRound;
    }

    private void Update()
    {
        roundDisplay.text = "Round: " + currentRound;
    }

    public void UpdateRound()
    {
        if (ScoreManager.GetScore() >= scoreRequirement)
        {
            currentRound++;
            excessScore = ScoreManager.GetScore() - scoreRequirement;
            foreach (GameObject thisObj in GameObject.FindGameObjectsWithTag("Peg"))
            {
                Destroy(thisObj);
            }
            onUpdate?.Invoke();
            scoreRequirement += currentRound * 256 + (excessScore / 2);
        }
    }


    public int GetCurrentRound() { return currentRound; }
    //called by ScoreManager to display the needed score
    public int GetScoreReq() { return scoreRequirement; }
    //called by LifeManager to calculate extra lives given
    public int GetExcessScore() { return excessScore; }
}
