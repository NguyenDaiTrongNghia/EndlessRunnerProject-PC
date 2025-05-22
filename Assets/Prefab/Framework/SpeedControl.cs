using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedControl : MonoBehaviour
{
    public delegate void OnGlobalSpeedChanged(float newSpeed);
    [SerializeField] float GlobalSpeed = 15f;
    //private float baseSpeed; // Store the base speed
    private bool isSpeedModified = false; // Flag to track active speed modification

    public event OnGlobalSpeedChanged onGlobalSpeedChanged;

    public event Action<float> OnSpeedBoostStarted;
    public event Action OnSpeedBoostEnded;
    //public delegate void OnSpeedBoostStarted(float newSpeed, float duration);
    //public event OnSpeedBoostStarted onSpeedBoostStarted;

    public void ChangeGlobalSpeed(float SpeedChange, float duration)
    {
        //Only apply speed change if no other speed modification is active
        if (!isSpeedModified)
        {
            isSpeedModified = true;
            GlobalSpeed += SpeedChange;
            InformSpeedChange();
            // Notify with duration
            OnSpeedBoostStarted?.Invoke(duration);

            StartCoroutine(RemoveSpeedChange(SpeedChange, duration));
        }
    }

    public float GetGlobalSpeed()
    {
        return GlobalSpeed;
    }

  IEnumerator RemoveSpeedChange(float SpeedChangeAmt, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GlobalSpeed -= SpeedChangeAmt;
        isSpeedModified = false;
        OnSpeedBoostEnded?.Invoke(); // Notify listeners
        InformSpeedChange();
    }

    private void InformSpeedChange()
    {
        onGlobalSpeedChanged?.Invoke(GlobalSpeed);
    }
}
