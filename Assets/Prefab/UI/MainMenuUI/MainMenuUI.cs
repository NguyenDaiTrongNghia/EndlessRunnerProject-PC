using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] UISwitcher menuSwitcher;
    [SerializeField] Transform mainMenu;
    [SerializeField] Transform howToPlayMenu;
    [SerializeField] Transform leaderBoardMenu;
    [SerializeField] Transform createPlayerProfileMenu;
    [SerializeField] TMP_InputField newPlayerNameField;
    [SerializeField] TMP_Dropdown playerList;
    [SerializeField] Transform storeMenu;

    [SerializeField] TextMeshProUGUI TotalCoins;
    [SerializeField] int TotalCoin;



    private void Start()
    {

        //PlayerPrefs.SetFloat("JumpBoostDuration", 70f);
        PlayerPrefs.Save();
        UpdatePlayerList();//
        playerList.onValueChanged.AddListener(UpdateSaveActivePlayer);//
        UpdateTotalCoinDisplay();
    }
    public void AddCoin()
    {
        PlayerPrefs.SetInt("TotalCoins", 5000);
        PlayerPrefs.Save();
        UpdateTotalCoinDisplay();
    }
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        UpdateTotalCoinDisplay();
    }
    public void UpdateTotalCoinDisplay()
    {
        TotalCoin = SaveDataManager.GetTotalCoins();
        TotalCoins.text = $"Total Coins: {TotalCoin}";
    }
    private void UpdateSaveActivePlayer(int index)
    {
        string currentActivePlayer = playerList.options[index].text;
        SaveDataManager.SetActivePlayer(currentActivePlayer);
    }

    private void UpdatePlayerList()//
    {
        SaveDataManager.GetSavedPlayersProfiles(out List<string> players);
        playerList.ClearOptions();
        playerList.AddOptions(players);
    }

    public void StartGame()//
    {
        GameplayStatics.GetGameMode().LoadFirstScene();
    }
    public void BackToMainMenu()//
    {
        menuSwitcher.SetActiveUI(mainMenu);
    }

    public void GoToHowToPlayMenu()//
    {
        menuSwitcher.SetActiveUI(howToPlayMenu);
    }

    public void GoToLeaderBoardMenu()//
    {
        menuSwitcher.SetActiveUI(leaderBoardMenu);
    }

    public void GoStoreMenu()//
    {
        menuSwitcher.SetActiveUI(storeMenu);
    }

    public void QuitGame()//
    {
        GameplayStatics.GetGameMode().QuitGame();
    }

    public void SwitchToPlayerProfileMenu()//
    {
        menuSwitcher.SetActiveUI(createPlayerProfileMenu);
    }

    public void AddPlayerProfile()//
    {
        string newPlayerName = newPlayerNameField.text;
        SaveDataManager.SavePlayerProfile(newPlayerName);
        UpdatePlayerList();
        BackToMainMenu();
    }

    public void DeleteSelectedPlayerProfile()//
    {
        if (playerList.options.Count != 0)
        {
            string playerName = playerList.options[playerList.value].text;
            SaveDataManager.DeletePlayerProfile(playerName);
            UpdatePlayerList();
        }       
    }
}
