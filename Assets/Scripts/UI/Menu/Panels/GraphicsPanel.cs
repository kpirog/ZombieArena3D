using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicsPanel : MonoBehaviour, ISettingsPanel
{
    [Header("Resolution Settings")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private List<string> resolutionNames = new List<string>();
    private List<Resolution> resolutions;
    private Resolution currentResolution;

    [Header("Fullscreen Settings")]
    [SerializeField] private Toggle fullscreenToggle;
    private bool isFullscreen;

    [Header("Quality Settings")]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    private int qualityIndex;

    [Header("VSync Settings")]
    [SerializeField] private Toggle vSyncToggle;
    private bool isVSyncOn = false;

    [Header("Brightness Settings")]
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private TMP_Text brightnessValueText;
    private float brightness;

    private void Awake()
    {
        resolutions = new List<Resolution>(Screen.resolutions);
        resolutions.ForEach(t => resolutionNames.Add(t.width.ToString() + " X " + t.height.ToString()));
        resolutionDropdown.AddOptions(resolutionNames);
    }
    public void SetResolution(int resolutionIndex)
    {
        currentResolution = resolutions[resolutionIndex];
    }
    public void SetFullscreen()
    {
        isFullscreen = !isFullscreen;
    }
    public void SetQuality(int qualityIndex)
    {
        this.qualityIndex = qualityIndex;
    }
    public void SetVSync()
    {
        isVSyncOn = !isVSyncOn;
    }
    public void SetBrightness(float brightness)
    {
        this.brightness = brightness;
        brightnessValueText.SetText(brightness.ToString("0.0"));
    }

    public void ApplySettings()
    {
        PlayerPrefs.SetInt("masterResolution", resolutions.IndexOf(currentResolution));
        Screen.SetResolution(currentResolution.width, currentResolution.height, isFullscreen);

        PlayerPrefs.SetInt("masterFullscreen", isFullscreen ? 1 : 0);
        Screen.fullScreen = isFullscreen;

        PlayerPrefs.SetInt("masterQuality", qualityIndex);
        QualitySettings.SetQualityLevel(qualityIndex);

        int vSyncCount = isVSyncOn ? 1 : 0;
        PlayerPrefs.SetInt("masterVSync", vSyncCount);
        QualitySettings.vSyncCount = vSyncCount;

        PlayerPrefs.SetFloat("masterBrightness", brightness);
        Screen.brightness = brightness;
    }

    public void ResetSettings()
    {
        SetResolution(resolutions.Count - 1);
        resolutionDropdown.value = resolutions.Count - 1;
        resolutionDropdown.RefreshShownValue();

        isFullscreen = true;
        fullscreenToggle.isOn = isFullscreen;

        SetQuality(0);
        qualityDropdown.value = qualityIndex;
        qualityDropdown.RefreshShownValue();

        isVSyncOn = false;
        vSyncToggle.isOn = isVSyncOn;

        SetBrightness(0.5f);
        brightnessSlider.value = brightness;

        ApplySettings();
    }
}
