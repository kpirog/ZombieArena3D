using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Stats statsModel;

    private float currentHealth;
    private float currentShield;

    public float CurrentHealth => currentHealth;
    public float CurrentShield => currentShield;
    public bool IsAlive => CurrentHealth > 0f;
    public Stats StatsModel => statsModel;

    private NeedBarsController needBarsController;

    private void Awake()
    {
        needBarsController = FindObjectOfType<NeedBarsController>();
        currentHealth = statsModel.MaxHealth;
        currentShield = statsModel.MaxShield;
    }
    [ContextMenu("TakeDamage")]
    public void TakeDamage()
    {
        if (IsAlive)
        {
            currentHealth -= 5f;
            needBarsController.onPlayerStatsUpdated?.Invoke();
        }
    }
    [ContextMenu("DecreaseShield")]
    public void DecreaseShield()
    {
        if (IsAlive)
        {
            currentShield -= 3f;
            needBarsController.onPlayerStatsUpdated?.Invoke();
        }
    }
}
