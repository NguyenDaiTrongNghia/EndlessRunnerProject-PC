using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBoost : Pickup
{
    [SerializeField] float JumpEffectDuration = 5.0f; 
    [SerializeField] float JumpEffect = 2.0f; 
    
    protected override void PickedUpBy(GameObject picker)
    {
        Player jumpController = picker.GetComponent<Player>();
        if (jumpController != null)
        {
            jumpController.ChangeJumpHeight(JumpEffect, JumpEffectDuration);
        }

        base.PickedUpBy(picker);
    }
}
