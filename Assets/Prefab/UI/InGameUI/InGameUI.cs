using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI CoinText;//
    [SerializeField] TextMeshProUGUI TimerText;//
    [SerializeField] UISwitcher menuSwitcher;
    [SerializeField] Transform inGameUI;
    [SerializeField] Transform pauseUI;
    [SerializeField] Transform gameoverUI;

    [Header("Power-Up UI Elements")]
    [SerializeField] PowerUpDisplay JumpBoostDisplay;
    [SerializeField] PowerUpDisplay SpeedBoostDisplay;
    [SerializeField] PowerUpDisplay MagnetDisplay;
    // Start is called before the first frame update

    void Start()
    {
        GameplayStatics.GetGameMode().onGameOver += OnGameOver;//

        ScoreKeeper scoreKeeper = FindObjectOfType<ScoreKeeper>();
        if(scoreKeeper != null)
        {
            scoreKeeper.onCoinChanged += UpdateCoinText;
            scoreKeeper.onTimeChanged += UpdateTimerText;
        }

        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.OnJumpBoostStarted += (duration) => JumpBoostDisplay.Show("Jump", duration);
            player.OnJumpBoostEnded += () => JumpBoostDisplay.Stop();
        }

        SpeedControl speedControl = FindObjectOfType<SpeedControl>();
        if (speedControl != null)
        {
            speedControl.OnSpeedBoostStarted += (duration) => SpeedBoostDisplay.Show("Speed", duration);
            speedControl.OnSpeedBoostEnded += () => SpeedBoostDisplay.Stop();
        }

        PlayerMagnetEffect Magnet = FindObjectOfType<PlayerMagnetEffect>();
        if (Magnet != null)
        {
            Magnet.OnMagnetStarted += (duration) => MagnetDisplay.Show("Magnet", duration);
            Magnet.OnMagnetEnded += () => MagnetDisplay.Stop();
        }

        MagnetDisplay.gameObject.SetActive(false);
        JumpBoostDisplay.gameObject.SetActive(false);
        SpeedBoostDisplay.gameObject.SetActive(false);
    }

    private void OnGameOver()//
    {
        menuSwitcher.SetActiveUI(gameoverUI);//
    }

    private void UpdateCoinText(int newVal)//
    {
        CoinText.SetText($"Coin: {newVal}");
    }

    private void UpdateTimerText(float newTime)//
    {
        TimerText.SetText($"Score: {newTime:F0}");
    }

    internal void SignalPause(bool isGamePaused)//
    {
        if (isGamePaused)
        {
            menuSwitcher.SetActiveUI(pauseUI);
        }
        else
        {
            menuSwitcher.SetActiveUI(inGameUI);
        }
    }

    public void ResumeGame()//
    {
        GameplayStatics.GetGameMode().SetGamePause(false);
        menuSwitcher.SetActiveUI(inGameUI);
    }

    public void BackToMainMenu()//
    {
        GameplayStatics.GetGameMode().BackToMainMenu();
    }

    public void RestartLevel()//
    {
        GameplayStatics.GetGameMode().RestartLevel();
    }
}
