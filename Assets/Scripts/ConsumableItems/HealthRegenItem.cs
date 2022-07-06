using UnityEngine;

public class HealthRegenItem : Consumable
{
    [Header("Health regen settings")]
    [SerializeField] private float timeToRegen;

    public override void Use()
    {
        if (playerStats.CurrentHealth < playerStats.StatsModel.MaxHealth)
        {
            Invoke(nameof(HealthRegen), timeToRegen);
        }
    }
    private void HealthRegen()
    {
        playerStats.AddHealth(ConsumableItem.HealthRegen);
        equipmentUI.SelectedSlot.UpdateConsumableAmount(false);

        if (equipmentUI.SelectedSlot.ConsumableAmount < 1)
        {
            Destroy(gameObject);
        }
    }
}
