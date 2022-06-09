using UnityEngine;
using UnityEngine.UI;

public class HudUI : MonoBehaviour
{
    [SerializeField] private Image defaultCrosshair;
    [SerializeField] private Image aimCrosshair;

    public void SetCrosshair(bool aimState)
    {
        defaultCrosshair.enabled = !aimState;
        aimCrosshair.enabled = aimState;
    }
}
