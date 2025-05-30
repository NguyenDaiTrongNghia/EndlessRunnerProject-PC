using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameplayStatics
{
    static GameMode gameMode;
    public static bool IsPositionOccupied(Vector3 position, Vector3 detectionHalfExtend, string occupationCheckTag, bool includeNoSpawn = true)
    {
        Collider[] cols = Physics.OverlapBox(position, detectionHalfExtend, Quaternion.identity);
        foreach (Collider col in cols)
        {
            if (col.gameObject.CompareTag(occupationCheckTag) || (includeNoSpawn && col.gameObject.CompareTag("NoSpawn")))
            {
                return true;
            }
        }
        return false;
    }

    public static GameMode GetGameMode()
    {
        if(gameMode == null)
        {
            gameMode = GameObject.FindObjectOfType<GameMode>();
        }
        return gameMode;
    }
}
