using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : ActionBaseState
{
    public override void EnterState(ActionStateManager action)
    {
        
    }

    public override void UpdateState(ActionStateManager action)
    {
        action.rHandRig.weight = Mathf.Lerp(action.rHandRig.weight, 1f, 10f * Time.deltaTime);
        action.lHandIK.weight = Mathf.Lerp(action.lHandIK.weight, 1f, 10f * Time.deltaTime);

        if (CanReload(action)) action.SwitchState(action.Reload);
    }
    private bool CanReload(ActionStateManager action) => action.reloadAction.triggered && action.currentWeapon.ammo.CanReload;
}
