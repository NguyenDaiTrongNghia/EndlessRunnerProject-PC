using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] LeaderBoardEntry leaderBoardEntryPrefab;
    [SerializeField] RectTransform leaderBoardList;
    private void OnEnable()
    {
        RefreshLeaderBoard();
    }

    private void RefreshLeaderBoard()
    {
        ClearLeaderBoardList();

        SaveDataManager.GetSavedLeaderBoardEntryList(out List<SaveDataManager.LeaderBoardEntryData> entries);
        foreach(var entryData in entries)
        {
            LeaderBoardEntry newEntry = Instantiate(leaderBoardEntryPrefab, leaderBoardList);
            newEntry.Init(entryData.name, entryData.date, entryData.score);
            leaderBoardList.sizeDelta += new Vector2(0, newEntry.GetComponent<RectTransform>().sizeDelta.y);
        }
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
