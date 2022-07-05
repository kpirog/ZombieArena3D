using UnityEngine;

public abstract class Consumable : Interactable
{
    [SerializeField] private Vector3 positionInHand;
    [SerializeField] private Vector3 rotationInHand;

    protected PlayerStats playerStats;
    public ConsumableItem ConsumableItem => ItemBase as ConsumableItem;

    protected override void Awake()
    {
        base.Awake();
        playerStats = FindObjectOfType<PlayerStats>();
    }
    private void Start()
    {
        if (IsInEquipment)
        {
            SetCorrectTransform();
        }
    }
    public abstract void Use();
    public virtual void SetCorrectTransform()
    {
        transform.localPosition = positionInHand;
        transform.localEulerAngles = rotationInHand;
    }
}
