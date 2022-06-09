using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultState : AimBaseState
{
    public override void EnterState(AimStateManager aim)
    {
        aim.anim.SetBool("Aiming", false);
        aim.currentFov = aim.defaultFov;
        aim.hudUI.SetCrosshair(false);
    }

    public override void UpdateState(AimStateManager aim)
    {
        if (aim.IsAiming) aim.SwitchState(aim.Aim);
    }
}
