using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.anim.SetBool("Walking", true);
    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (movement.direction.magnitude < 0.1f) ExitState(movement, movement.Idle);
        if (movement.IsSprinting) ExitState(movement, movement.Run);
        if (movement.isCrouching) ExitState(movement, movement.Crouch);

        if (movement.input.y < 0f) movement.currentSpeed = movement.walkBackSpeed;
        else movement.currentSpeed = movement.walkSpeed;
    }
    private void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.anim.SetBool("Walking", false);
        movement.SwitchState(state);
    }
}
