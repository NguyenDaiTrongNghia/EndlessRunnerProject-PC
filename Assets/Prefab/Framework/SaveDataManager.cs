using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public static class SaveDataManager
{
    [Serializable]
    class PlayerProfilesData
    {
        public List<string> playerNames;
        public PlayerProfilesData(List<string> names)
        {
            playerNames = names;
        }
    }

    [Serializable]
    public class LeaderBoardEntryData
    {
        public string name;
        public string date;
        public float score;

        public LeaderBoardEntryData(string name, DateTime date, float score)
        {
            this.name = name;
            this.date = date.ToString();
            this.score = score;
        }
    }

    [Serializable]
    class LeaderboardListData
    {
        public List<LeaderBoardEntryData> entries = new List<LeaderBoardEntryData>();
        public LeaderboardListData(List<LeaderBoardEntryData> entries)
        {
            this.entries = entries;
        }
    }
    static string GetSaveDir()//
    {
        return Application.persistentDataPath;
    }

    static string GetPlayerProfileFileName()//
    {
        return "players.json";
    }

    static string GetPlayerProfileSaveDir()//
    {
        return GetSaveDir() + "/" + GetPlayerProfileFileName();
    }

    public static void SavePlayerProfile(string playerName)//
    {
        GetSavedPlayersProfiles(out List<string> players);
        if (players.Contains(playerName))
        {
            return;
        }
        players.Insert(0, playerName);
        SavePlayerProfilesFromList(players);
    }

    private static void SavePlayerProfilesFromList(List<string> players)//
    {
        PlayerProfilesData data = new PlayerProfilesData(players);
        string dataJSON = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPlayerProfileSaveDir(), dataJSON);
    }

    public static bool GetSavedPlayersProfiles(out List<string> data)
    {
        if (File.Exists(GetPlayerProfileSaveDir()))//
        {
            string dataJSON = File.ReadAllText(GetPlayerProfileSaveDir());//
            PlayerProfilesData loadedData = JsonUtility.FromJson<PlayerProfilesData>(dataJSON);//
            data = loadedData.playerNames;//
            return true;//
        }
        data = new List<string>();//
        return false;        //
    }

    public static void DeletePlayerProfile(string playerName)//
    {
        GetSavedPlayersProfiles(out List<string> players);//
        players.Remove(playerName);//
        SavePlayerProfilesFromList(players);//
    }
    
    public static void SaveNewLeadBoardEntry(string name, DateTime date, float score)
    {
        LeaderBoardEntryData newEntry = new LeaderBoardEntryData(name, date, score);//
        GetSavedLeaderBoardEntryList(out List<LeaderBoardEntryData> entries);//

        int existingIndex = entries.FindIndex(e => e.name == name);

        if (existingIndex != -1)
        {
            if (score > entries[existingIndex].score)
            {
                entries[existingIndex] = newEntry;
            }
            else
            {
                return;
            }
        }
        else
        {
            entries.Add(newEntry);
        }

        // Sort algorithm
        entries.Sort((a, b) => b.score.CompareTo(a.score));


        LeaderboardListData data = new LeaderboardListData(entries);//
        string dataJSON = JsonUtility.ToJson(data, true);//
        File.WriteAllText(GetLeaderBoardSaveDir(), dataJSON);//
    }
    public static bool GetSavedLeaderBoardEntryList(out List<LeaderBoardEntryData> entries)//
    {
        if (File.Exists(GetLeaderBoardSaveDir()))
        {
            string loadedDataJSON = File.ReadAllText(GetLeaderBoardSaveDir());
            LeaderboardListData loadedData = JsonUtility.FromJson<LeaderboardListData>(loadedDataJSON);
            entries = loadedData.entries;
            return true;
        }

        entries = new List<LeaderBoardEntryData>();
        return false;
    }

    private static string GetLeaderBoardSaveDir()//
    {
        return GetSaveDir() + "/" + GetLeaderBoardFileName();
    }

    private static string GetLeaderBoardFileName()//
    {
        return "LeaderBoard.lb";
    }

    public static void SetActivePlayer(string playerName)//
    {   
        GetSavedPlayersProfiles(out List<string> players);
        if (players.Remove(playerName))
        {
            players.Insert(0, playerName);
            SavePlayerProfilesFromList(players);
        }       
    }

    public static string GetActivePlayerName()//
    {
        GetSavedPlayersProfiles(out List<string> players);
        if(players.Count != 0)
        {
            return players[0];
        }
        return "Anonymous Player";
    }

    private const string TotalCoinsKey = "TotalCoins";

    //Coin System
    public static int GetTotalCoins()
    {
        return PlayerPrefs.GetInt("TotalCoins", 0);
    }

    public static void AddCoins(int coinsToAdd)
    {
        int current = GetTotalCoins();
        PlayerPrefs.SetInt("TotalCoins", current + coinsToAdd);
        PlayerPrefs.Save();
    }

    public static void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }

    // Power-up upgrade keys
    public static void SavePowerUpDuration(string key, float duration)
    {
        PlayerPrefs.SetFloat(key, duration);
        PlayerPrefs.Save();
    }

    public static float GetPowerUpDuration(string key, float defaultDuration = 5.0f)
    {
        return PlayerPrefs.GetFloat(key, defaultDuration);
    }
}
