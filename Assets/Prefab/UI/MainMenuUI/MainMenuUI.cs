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
<<<<<<< Updated upstream
=======
    [SerializeField] Transform skinShop;
    [SerializeField] GameObject PlayerNameExistPanel;
>>>>>>> Stashed changes

    [SerializeField] TextMeshProUGUI TotalCoins;
    [SerializeField] int TotalCoin;

    public static MainMenuUI Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ClosePlayerNameExistPanel();
        //PlayerPrefs.SetFloat("JumpBoostDuration", 70f);
        PlayerPrefs.Save();
        UpdatePlayerList();//
        playerList.onValueChanged.AddListener(UpdateSaveActivePlayer);//
        UpdateTotalCoinDisplay();

        // Check if any player profiles exist; if not, force create profile menu
        if (!SaveDataManager.HasPlayerProfiles())
        {
            menuSwitcher.SetActiveUI(createPlayerProfileMenu);
            PlayerProfileMenu.gameObject.SetActive(false);
        }
    }

    public void DisplayPlayerNameExistPanel()
    {
        PlayerNameExistPanel.SetActive(true);
    }
  
    public void ClosePlayerNameExistPanel()
    {
        PlayerNameExistPanel.SetActive(false);
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
        // Check if an active player exists
        string activePlayer = SaveDataManager.GetActivePlayerName();
        if (string.IsNullOrEmpty(activePlayer))
        {
            // No profile exists, redirect to create profile menu
            menuSwitcher.SetActiveUI(createPlayerProfileMenu);
            PlayerProfileMenu.gameObject.SetActive(false);
        }
        else
        {
            // Profile exists, start the game
            GameplayStatics.GetGameMode().LoadFirstScene();
        }
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
        SaveDataManager.SavePlayerProfile(newPlayerName, () =>
        {
            UpdatePlayerList();
            BackToMainMenu();
        });
    }

    public void DeleteSelectedPlayerProfile()//
    {
        if (playerList.options.Count != 0)
        {
            string playerName = playerList.options[playerList.value].text;
            SaveDataManager.DeletePlayerProfile(playerName);
            FirebaseManager.Instance.DeletePlayerProfileFromFirebase(playerName, () =>
            {
                // After deleting player from Firebase, remove associated leaderboard entries
                FirebaseManager.Instance.DeleteLeaderboardEntriesByName(playerName, () =>
                {
                    UpdatePlayerList();
                });
            });
        }
    }
}
