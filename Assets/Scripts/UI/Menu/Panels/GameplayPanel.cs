using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayPanel : MonoBehaviour, ISettingsPanel
{
    [SerializeField] private Slider mouseSensSlider;
    [SerializeField] private TMP_Text mouseSensValueText;
    private float currentSens = 1.0f;

    [SerializeField] private Toggle invertYToggle;
    private bool isMouseInverted = true;

    public void SetSensitivity(float sensitivity)
    {
        currentSens = sensitivity;
        mouseSensValueText.SetText(sensitivity.ToString("0.0"));
    }
    public void SetInvertYToggle()
    {
        isMouseInverted = !isMouseInverted;
    }
    
    public void ApplySettings()
    {
        PlayerPrefs.SetFloat("masterSensitivity", currentSens);
        PlayerPrefs.SetInt("masterInvertY", isMouseInverted ? 1 : 0);
    }

    public void ResetSettings()
    {
        SetSensitivity(1.0f);
        mouseSensSlider.value = currentSens;

        isMouseInverted = true;
        invertYToggle.isOn = isMouseInverted;
        
        ApplySettings();
    }
}
