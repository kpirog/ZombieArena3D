using UnityEngine;

public class HealthBar : BaseNeedBar
{
    public override float FillValue => playerStats.CurrentHealth;
    public override float MaxFillValue => playerStats.StatsModel.MaxHealth;

    protected override void UpdateUI()
    {
        base.UpdateUI();
    }
}
