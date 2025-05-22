using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    public delegate void OnScoreChanged(int newVal);
    public event OnScoreChanged onScoreChanged;
    private int score;

    public delegate void OnTimeChanged(float newTime);
    public event OnTimeChanged onTimeChanged;
    public float CurrentTime;

    public void ChangeScore(int amt)
    {
        score += amt;
        //Debug.Log($"The score has changed {score}");
        onScoreChanged?.Invoke(score);
    }

    void Update()
    {
        CurrentTime = CurrentTime += Time.deltaTime;
        onTimeChanged?.Invoke(CurrentTime);
    }
    
}
