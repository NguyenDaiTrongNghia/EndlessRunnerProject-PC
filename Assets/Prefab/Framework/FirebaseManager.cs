using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using TMPro;

public class FirebaseManager : MonoBehaviour
{
    
    [SerializeField] GameObject NewHighScorePanel;
    [SerializeField] TextMeshProUGUI NewHighScoreText;

    private DatabaseReference dbReference;

    public static FirebaseManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

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
                //Debug.LogError("Firebase not available: " + task.Result);
            }
        });
    }

    public void SavePlayerProfileToFirebase(string playerName, Action onComplete = null)
    {
        dbReference.Child("players").Child(playerName).SetValueAsync(true).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                //Debug.LogError("Error saving player profile to Firebase: " + task.Exception);
            }
            else
            {
                //Debug.Log($"Player {playerName} saved to Firebase.");
                onComplete?.Invoke();
            }
        });
    }

    public void CheckPlayerExists(string playerName, Action<bool> callback)
    {
        dbReference.Child("players").Child(playerName).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                //Debug.LogError("Error checking player existence: " + task.Exception);
                callback?.Invoke(false);
            }
            else
            {
                callback?.Invoke(task.Result.Exists);
            }
        });
    }
    public void DeletePlayerProfileFromFirebase(string playerName, Action onComplete = null)
    {
        dbReference.Child("players").Child(playerName).RemoveValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error deleting player profile from Firebase: " + task.Exception);
            }
            else
            {
                //Debug.Log($"Player {playerName} deleted from Firebase.");
                onComplete?.Invoke();
            }
        });
    }

    public void DeleteLeaderboardEntriesByName(string playerName, Action onComplete = null)
    {
        dbReference.Child("leaderboard").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                //Debug.LogError("Error fetching leaderboard for deletion: " + task.Exception);
                onComplete?.Invoke();
                return;
            }

            DataSnapshot snapshot = task.Result;
            List<string> keysToDelete = new List<string>();

            foreach (DataSnapshot child in snapshot.Children)
            {
                string name = child.Child("name").Value.ToString();
                if (name == playerName)
                {
                    keysToDelete.Add(child.Key);
                }
            }

            if (keysToDelete.Count > 0)
            {
                var tasks = new List<Task>();
                foreach (string key in keysToDelete)
                {
                    tasks.Add(dbReference.Child("leaderboard").Child(key).RemoveValueAsync());
                }

                Task.WhenAll(tasks).ContinueWithOnMainThread(t =>
                {
                    if (t.IsFaulted)
                    {
                        //Debug.LogError("Error deleting leaderboard entries: " + t.Exception);
                    }
                    else
                    {
                        //Debug.Log($"Deleted {keysToDelete.Count} leaderboard entries for {playerName}.");
                    }
                    onComplete?.Invoke();
                });
            }
            else
            {
                onComplete?.Invoke();
            }
        });
    }


    public void UploadLeaderboardEntry(string name, DateTime date, float score)
    {
        dbReference.Child("leaderboard").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                //Debug.LogError("Error fetching leaderboard before upload: " + task.Exception);
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
                    NewHighScorePanel.SetActive(true);
                    NewHighScoreText.SetText($"New High Score: {score:F0}");
                    // Debug.Log("Updated leaderboard entry with higher score.");
                }
            }
            else
            {
                string key = dbReference.Child("leaderboard").Push().Key;
                SaveDataManager.LeaderBoardEntryData newEntry = new SaveDataManager.LeaderBoardEntryData(name, date, score);
                string json = JsonUtility.ToJson(newEntry);
                dbReference.Child("leaderboard").Child(key).SetRawJsonValueAsync(json);
                //Debug.Log("New leaderboard entry created.");
            }
        });
    }
    public void FetchLeaderboardEntries(Action<List<SaveDataManager.LeaderBoardEntryData>> callback)
    {
        dbReference.Child("leaderboard").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                //Debug.LogError("Error fetching leaderboard: " + task.Exception);
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
