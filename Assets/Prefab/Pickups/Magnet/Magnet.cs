using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Pickup
{
    [SerializeField] float magnetDuration = 5f;
    private void Start()
    {
        magnetDuration = SaveDataManager.GetPowerUpDuration("MagnetDuration");
    }
    protected override void PickedUpBy(GameObject picker)
    {
        PlayerMagnetEffect magnet = picker.GetComponent<PlayerMagnetEffect>();
        if (magnet != null)
        {
            magnet.ActivateMagnet(magnetDuration);
        }

        base.PickedUpBy(picker);
    }

}
