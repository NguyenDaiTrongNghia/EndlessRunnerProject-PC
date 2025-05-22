using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Pickup
{
    protected override void PickedUpBy(GameObject picker)
    {
        PlayerMagnetEffect magnet = picker.GetComponent<PlayerMagnetEffect>();
        if (magnet != null)
        {
            magnet.ActivateMagnet();
        }

        base.PickedUpBy(picker);
    }

}
