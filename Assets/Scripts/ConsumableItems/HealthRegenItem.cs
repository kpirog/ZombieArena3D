using System.Collections;
using UnityEngine;

public class HealthRegenItem : Consumable
{
    [Header("Health regen settings")]
    [SerializeField] private float timeToRegen;

    public override void Use()
    {
        if (playerStats.CurrentHealth < playerStats.StatsModel.MaxHealth)
        {
            StartCoroutine(RegenHealthAfterTime());
        }
    }
    private IEnumerator RegenHealthAfterTime()
    {
        yield return new WaitForSeconds(timeToRegen);
        playerStats.AddHealth(ConsumableItem.HealthRegen);
        
        equipmentUI.SelectedSlot.ItemBase = null;
        equipmentUI.SelectedSlot.UpdateSlotUI();
        
        Destroy(gameObject);
    }
}
