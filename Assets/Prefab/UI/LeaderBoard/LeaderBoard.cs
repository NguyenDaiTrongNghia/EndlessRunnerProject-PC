using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] LeaderBoardEntry leaderBoardEntryPrefab;
    [SerializeField] RectTransform leaderBoardList;

    private DatabaseReference dbReference;
    private void Awake()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void OnEnable()
    {
        RefreshLeaderBoard();
    }

    private void RefreshLeaderBoard()
    {
        ClearLeaderBoardList();

        FirebaseDatabase.DefaultInstance
            .GetReference("leaderboard")
            .OrderByChild("score")
            .LimitToLast(10)
            .GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("Failed to load leaderboard data.");
                    return;
                }

                DataSnapshot snapshot = task.Result;
                List<LeaderBoardEntryData> entries = new List<LeaderBoardEntryData>();

                foreach (DataSnapshot child in snapshot.Children)
                {
                    string name = child.Child("name").Value.ToString();
                    string date = child.Child("date").Value.ToString();
                    float score = float.Parse(child.Child("score").Value.ToString());

                    entries.Add(new LeaderBoardEntryData(name, date, score));
                }

                // Optional: Sort descending
                entries.Sort((a, b) => b.score.CompareTo(a.score));

                foreach (var entryData in entries)
                {
                    LeaderBoardEntry newEntry = Instantiate(leaderBoardEntryPrefab, leaderBoardList);
                    newEntry.Init(entryData.name, entryData.date, entryData.score);
                    leaderBoardList.sizeDelta += new Vector2(0, newEntry.GetComponent<RectTransform>().sizeDelta.y);
                }
            });
    }

    private void ClearLeaderBoardList()
    {
        leaderBoardList.sizeDelta = new Vector2(leaderBoardList.sizeDelta.x, 0);
        foreach(Transform entry in leaderBoardList)
        {
            Destroy(entry.gameObject);
        }
    }

    private class LeaderBoardEntryData
    {
        public string name;
        public string date;
        public float score;

        public LeaderBoardEntryData(string name, string date, float score)
        {
            this.name = name;
            this.date = date;
            this.score = score;
        }
    }
}
