using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] LeaderBoardEntry leaderBoardEntryPrefab;
    [SerializeField] RectTransform leaderBoardList;
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
    private void OnEnable()
    {
        RefreshLeaderBoard();
    }

    private void RefreshLeaderBoard()
    {
        ClearLeaderBoardList();
<<<<<<< Updated upstream

        SaveDataManager.GetSavedLeaderBoardEntryList(out List<SaveDataManager.LeaderBoardEntryData> entries);
        foreach(var entryData in entries)
        {
            LeaderBoardEntry newEntry = Instantiate(leaderBoardEntryPrefab, leaderBoardList);
            newEntry.Init(entryData.name, entryData.date, entryData.score);
            leaderBoardList.sizeDelta += new Vector2(0, newEntry.GetComponent<RectTransform>().sizeDelta.y);
        }
=======
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
                if (!snapshot.Exists || snapshot.ChildrenCount == 0)
                {
                    Debug.LogWarning("Leaderboard snapshot does not exist or is empty.");
                    return;
                }
                List<LeaderBoardEntryData> entries = new List<LeaderBoardEntryData>();

                foreach (DataSnapshot child in snapshot.Children)
                {
                    string name = child.Child("name").Value.ToString();
                    string date = child.Child("date").Value.ToString();
                    float score = float.Parse(child.Child("score").Value.ToString());

                    entries.Add(new LeaderBoardEntryData(name, date, score));
                }

                //Sort descending
                entries.Sort((a, b) => b.score.CompareTo(a.score));

                foreach (var entryData in entries)
                {
                    LeaderBoardEntry newEntry = Instantiate(leaderBoardEntryPrefab, leaderBoardList);
                    newEntry.Init(entryData.name, entryData.date, entryData.score);
                    leaderBoardList.sizeDelta += new Vector2(0, newEntry.GetComponent<RectTransform>().sizeDelta.y);
                }
            });
>>>>>>> Stashed changes
    }

    private void ClearLeaderBoardList()
    {
        leaderBoardList.sizeDelta = new Vector2(leaderBoardList.sizeDelta.x, 0);
        foreach(Transform entry in leaderBoardList)
        {
            Destroy(entry.gameObject);
        }
    }
}
