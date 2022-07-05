using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Animations.Rigging;

public class RigController : MonoBehaviour
{
    [SerializeField] private MultiAimConstraint rHandRig;
    [SerializeField] private MultiAimConstraint bodyRig;
    [SerializeField] private MultiAimConstraint headRig;

    [SerializeField] private TwoBoneIKConstraint rHandIK;
    [SerializeField] private TwoBoneIKConstraint lHandIK;

    public UnityAction<bool> onRigChanged = delegate { };

    private void OnEnable() => onRigChanged += SetRigWeight;
    private void OnDisable() => onRigChanged -= SetRigWeight;

    private void SetRigWeight(bool state)
    {
        rHandRig.weight = state ? 1f : 0f;
        bodyRig.weight = state ? 0.5f : 0f;
        rHandIK.weight = state ? 1f : 0f;
        lHandIK.weight = state ? 1f : 0f;
    }
}
