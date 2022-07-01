using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class BaseNeedBar : MonoBehaviour
{
    [SerializeField] private Image barFillImage;
    [SerializeField] private TMP_Text statusText;

    protected NeedBarsController needBarsController;
    protected PlayerStats playerStats;
    public abstract float FillValue { get; }
    public abstract float MaxFillValue { get; }

    private void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        needBarsController = GetComponentInParent<NeedBarsController>();
    }
    private void OnEnable() => needBarsController.onPlayerStatsUpdated.AddListener(UpdateUI);

    private void OnDisable() => needBarsController.onPlayerStatsUpdated.RemoveAllListeners();

    protected virtual void UpdateUI()
    {
        barFillImage.fillAmount = FillValue / MaxFillValue;
        statusText.text = FillValue.ToString("0") + " / " + MaxFillValue.ToString("0");
    }
}
