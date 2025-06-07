using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedControl : MonoBehaviour
{
    public delegate void OnGlobalSpeedChanged(float newSpeed);//
    [SerializeField] float GlobalSpeed = 15f;//
    private bool isSpeedModified = false; //
    private Coroutine speedBoostCoroutine;

    public event OnGlobalSpeedChanged onGlobalSpeedChanged;//

    public event Action<float> OnSpeedBoostStarted;//
    public event Action OnSpeedBoostEnded;//

    public void ChangeGlobalSpeed(float SpeedChange, float duration)//
    {
        
        if (!isSpeedModified)
        {
            isSpeedModified = true;
            GlobalSpeed += SpeedChange;
        }
        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
        }
        InformSpeedChange();
        OnSpeedBoostStarted?.Invoke(duration);
        speedBoostCoroutine = StartCoroutine(RemoveSpeedChange(SpeedChange, duration));
    }

    IEnumerator RemoveSpeedChange(float SpeedChangeAmt, float waitTime)//
    {
        yield return new WaitForSeconds(waitTime);
        GlobalSpeed -= SpeedChangeAmt;
        isSpeedModified = false;
        speedBoostCoroutine = null;
        OnSpeedBoostEnded?.Invoke();
        InformSpeedChange();
    }


    public float GetGlobalSpeed()//
    {
        return GlobalSpeed;
    }

    private void InformSpeedChange()//
    {
        onGlobalSpeedChanged?.Invoke(GlobalSpeed);
    }

    //Change global speed to 20 after 300 seconds
    public void SetGlobalSpeedAfter300s(float newspeed)
    {
        GlobalSpeed = newspeed;
        onGlobalSpeedChanged?.Invoke(GlobalSpeed);
    }
    void SetGameSpeedAfter300s()
    {
        SetGlobalSpeedAfter300s(20);
    }
    private void Start()
    {
        Invoke("SetGameSpeedAfter300s", 300);
    }
}
