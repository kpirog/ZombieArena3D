using UnityEngine;

[CreateAssetMenu(fileName = "New Stats", menuName = "Stats/StatsModel")]
public class Stats : ScriptableObject
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxShield;

    public float MaxHealth => maxHealth;
    public float MaxShield => maxShield;
}
