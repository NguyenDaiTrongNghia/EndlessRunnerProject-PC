using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;

public class FirebaseManager : MonoBehaviour
{
    private DatabaseReference dbReference;

    public static FirebaseManager Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                dbReference = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                Debug.LogError("Firebase not available: " + task.Result);
            }
        });
    }

    public void UploadLeaderboardEntry(string name, DateTime date, float score)
    {
        dbReference.Child("leaderboard").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error fetching leaderboard before upload: " + task.Exception);
                return;
            }

            bool entryExists = false;
            string existingKey = null;
            float existingScore = 0;

            foreach (DataSnapshot child in task.Result.Children)
            {
                string json = child.GetRawJsonValue();
                var entry = JsonUtility.FromJson<SaveDataManager.LeaderBoardEntryData>(json);

                if (entry.name == name)
                {
                    entryExists = true;
                    existingKey = child.Key;
                    existingScore = entry.score;
                    break;
                }
            }

            if (entryExists)
            {
                if (score > existingScore)
                {
                    SaveDataManager.LeaderBoardEntryData updatedEntry = new SaveDataManager.LeaderBoardEntryData(name, date, score);
                    string json = JsonUtility.ToJson(updatedEntry);
                    dbReference.Child("leaderboard").Child(existingKey).SetRawJsonValueAsync(json);
                    Debug.Log("Updated leaderboard entry with higher score.");
                }
                else
                {
                    Debug.Log("New score is not higher. Not updating leaderboard.");
                }
            }
            else
            {
                string key = dbReference.Child("leaderboard").Push().Key;
                SaveDataManager.LeaderBoardEntryData newEntry = new SaveDataManager.LeaderBoardEntryData(name, date, score);
                string json = JsonUtility.ToJson(newEntry);
                dbReference.Child("leaderboard").Child(key).SetRawJsonValueAsync(json);
                Debug.Log("New leaderboard entry created.");
            }
        });
    }
    public void FetchLeaderboardEntries(Action<List<SaveDataManager.LeaderBoardEntryData>> callback)
    {
        dbReference.Child("leaderboard").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error fetching leaderboard: " + task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;
            List<SaveDataManager.LeaderBoardEntryData> entries = new List<SaveDataManager.LeaderBoardEntryData>();

            foreach (DataSnapshot child in snapshot.Children)
            {
                string json = child.GetRawJsonValue();
                var entry = JsonUtility.FromJson<SaveDataManager.LeaderBoardEntryData>(json);
                entries.Add(entry);
            }

            // Sort by score (descending)
            entries.Sort((a, b) => b.score.CompareTo(a.score));
            callback?.Invoke(entries);
        });
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
