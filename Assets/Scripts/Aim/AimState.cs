using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimState : AimBaseState
{
    public override void EnterState(AimStateManager aim)
    {
        aim.anim.SetBool("Aiming", true);
        aim.currentFov = aim.aimFov;
        aim.hudUI.SetCrosshair(true);
    }

    public override void UpdateState(AimStateManager aim)
    {
        if (!aim.IsAiming) aim.SwitchState(aim.Default);
    }
}
