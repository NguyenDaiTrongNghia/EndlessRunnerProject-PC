using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    public delegate void OnCoinChanged(int newVal);
    public event OnCoinChanged onCoinChanged;
    private int coin;

    public delegate void OnTimeChanged(float newTime);
    public event OnTimeChanged onTimeChanged;
    public float timeScore;

    public float TimeScore
    {
        get { return timeScore; }
    }

    public void ChangeCoin(int amt)
    {
        coin += amt;
        if(coin < 0)
        {
            coin = 0;
        }
        //Debug.Log($"The score has changed {score}");
        onCoinChanged?.Invoke(coin);
    }

    void Update()
    {
        timeScore += Time.deltaTime;
        onTimeChanged?.Invoke(timeScore);
    }
    
}
