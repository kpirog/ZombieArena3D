using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.anim.SetBool("Running", true);
    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (!movement.IsSprinting) ExitState(movement, movement.Walk);

        if (movement.input.y < 0f) movement.currentSpeed = movement.runBackSpeed;
        else movement.currentSpeed = movement.runSpeed;
    }
    private void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.anim.SetBool("Running", false);
        movement.SwitchState(state);
    }
}
