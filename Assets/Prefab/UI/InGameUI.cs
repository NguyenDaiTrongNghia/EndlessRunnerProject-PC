using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI TimerText;
    [Header("Power-Up UI Elements")]
    [SerializeField] PowerUpDisplay JumpBoostDisplay;
    [SerializeField] PowerUpDisplay SpeedBoostDisplay;
    [SerializeField] PowerUpDisplay MagnetDisplay;
    // Start is called before the first frame update

    void Start()
    {
        ScoreKeeper scoreKeeper = FindObjectOfType<ScoreKeeper>();
        if(scoreKeeper != null)
        {
            scoreKeeper.onScoreChanged += UpdateScoreText;
            scoreKeeper.onTimeChanged += UpdateTimerText;
        }

        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.OnJumpBoostStarted += (duration) => JumpBoostDisplay.Show("Jump: ", duration);
            player.OnJumpBoostEnded += () => JumpBoostDisplay.Stop();
        }

        SpeedControl speedControl = FindObjectOfType<SpeedControl>();
        if (speedControl != null)
        {
            speedControl.OnSpeedBoostStarted += (duration) => SpeedBoostDisplay.Show("Speed: ", duration);
            speedControl.OnSpeedBoostEnded += () => SpeedBoostDisplay.Stop();
        }

        PlayerMagnetEffect Magnet = FindObjectOfType<PlayerMagnetEffect>();
        if (Magnet != null)
        {
            Magnet.OnMagnetStarted += (duration) => MagnetDisplay.Show("Magnet: ", duration);
            Magnet.OnMagnetEnded += () => MagnetDisplay.Stop();
        }


        // Hide all power-up UIs initially
        MagnetDisplay.gameObject.SetActive(false);
        JumpBoostDisplay.gameObject.SetActive(false);
        SpeedBoostDisplay.gameObject.SetActive(false);
    }

    private void UpdateScoreText(int newVal)
    {
        ScoreText.SetText($"Coin: {newVal}");
    }

    private void UpdateTimerText(float newTime)
    {
        TimerText.SetText($"Score: {newTime:F0}");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
