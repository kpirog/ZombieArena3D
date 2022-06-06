using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBloom : MonoBehaviour
{
    [SerializeField] private float defaultBloomAngle = 3f;
    [SerializeField] private float runBloom = 2f;
    [SerializeField] private float walkBloom = 1.5f;
    [SerializeField] private float crouchBloom = 0.5f;
    [SerializeField] private float aimBloom = 0.5f;

    private float currentBloom;

    private MovementStateManager movement;
    private AimStateManager aim;

    private void Start()
    {
        movement = GetComponentInParent<MovementStateManager>();
        aim = GetComponentInParent<AimStateManager>();
    }
    public Vector3 SetBloom(Transform barrelTransform)
    {
        if (movement.currentState == movement.Idle) currentBloom = defaultBloomAngle;
        else if (movement.currentState == movement.Walk) currentBloom = defaultBloomAngle * walkBloom;
        else if (movement.currentState == movement.Run) currentBloom = defaultBloomAngle * runBloom;
        else if (movement.currentState == movement.Crouch)
        {
            if(movement.direction.magnitude > 0f) currentBloom = defaultBloomAngle * walkBloom * crouchBloom;
            else currentBloom = defaultBloomAngle * crouchBloom;
        }

        if (aim.currentState == aim.Aim) currentBloom *= aimBloom;

        float randX = Random.Range(-currentBloom, currentBloom);
        float randY = Random.Range(-currentBloom, currentBloom);
        float randZ = Random.Range(-currentBloom, currentBloom);

        Vector3 randomRotation = new Vector3(randX, randY, randZ);

        return barrelTransform.localEulerAngles + randomRotation;
    }
}
