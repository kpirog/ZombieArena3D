using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadState : ActionBaseState
{
    public override void EnterState(ActionStateManager action)
    {
        action.anim.SetTrigger("Reload");
    }

    public override void UpdateState(ActionStateManager action)
    {
        if (action.equipmentUI.EquippedItem != null && action.equipmentUI.EquippedItem is not ConsumableItem)
        {
            action.rHandRig.weight = Mathf.Lerp(action.rHandRig.weight, 0f, 10f * Time.deltaTime);
            action.lHandIK.weight = Mathf.Lerp(action.lHandIK.weight, 0f, 10f * Time.deltaTime);
        }
    }
}
