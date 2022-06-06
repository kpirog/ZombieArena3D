using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.anim.SetBool("Crouching", true);
    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (movement.direction.magnitude > 0.1f && movement.IsSprinting) ExitState(movement, movement.Run);
        else if(movement.direction.magnitude > 0.1f && !movement.isCrouching) ExitState(movement, movement.Walk);
        else if(movement.direction.magnitude < 0.1f && !movement.isCrouching) ExitState(movement, movement.Idle);

        if (movement.input.y < 0f) movement.currentSpeed = movement.crouchBackSpeed;
        else movement.currentSpeed = movement.crouchSpeed;
    }
    private void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.anim.SetBool("Crouching", false);
        movement.SwitchState(state);
    }
}
