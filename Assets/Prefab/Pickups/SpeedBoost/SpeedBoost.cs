using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : Pickup
{
    [SerializeField] float SpeedEffect;
    [SerializeField] float SpeedEffectDuration;
    private void Start()
    {
        SpeedEffectDuration = SaveDataManager.GetPowerUpDuration("SpeedBoostDuration");
    }
    protected override void PickedUpBy(GameObject picker)
    {
        SpeedControl speedController = FindObjectOfType<SpeedControl>();
        if (speedController != null && SpeedEffect != 0)
        {
            speedController.ChangeGlobalSpeed(SpeedEffect, SpeedEffectDuration);
        }

        base.PickedUpBy(picker);
    }
}
