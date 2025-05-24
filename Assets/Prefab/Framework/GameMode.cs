using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    public delegate void OnGameOver();

    [SerializeField] int MainMenuSceneBuildIndex = 0;
    [SerializeField] int FirstSceneIndex = 1;

    public event OnGameOver onGameOver;

    bool bIsGamePause = false;
    bool bIsGameOver = false;
    public void GameOver()
    {
        SetGamePause(true);
        bIsGameOver = true;
        onGameOver?.Invoke();
    }

    internal void LoadFirstScene()
    {
        LoadScene(FirstSceneIndex);
    }

    public void SetGamePause(bool bIsPaused)
    {
        if (bIsGameOver)
        {
            return;
        }

        bIsGamePause = bIsPaused;
        if (bIsGamePause)
        {
            Time.timeScale = 0;

        }
        else
        {
            Time.timeScale = 1;
        }
    }

    internal void TogglePause()
    {
        if (IsGamePaused())
        {
            SetGamePause(false);
        } else
        {
            SetGamePause(true);
        }
    }

    public bool IsGamePaused()
    {
        return bIsGamePause;
    }

    public void BackToMainMenu()
    {
        LoadScene(MainMenuSceneBuildIndex);
    }

    private void LoadScene(int sceneBuidIndex)
    {
        bIsGameOver = false;
        SetGamePause(false);
        SceneManager.LoadScene(sceneBuidIndex);
    }

    internal void RestartLevel()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    internal bool IsGameOver()
    {
        return bIsGameOver;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
