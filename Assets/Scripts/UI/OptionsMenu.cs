using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    [SerializeField] Slider volumeSlider;
    [SerializeField] TMP_Dropdown graphicsDropdown;
    [SerializeField] Toggle fullscreenToggle;

    void Start()
    {
        volumeSlider.value = PlayerPrefs.HasKey("volume") ? PlayerPrefs.GetFloat("volume") : 0f;
        graphicsDropdown.value = PlayerPrefs.HasKey("quality") ? PlayerPrefs.GetInt("quality") : 0;
        fullscreenToggle.isOn = PlayerPrefs.HasKey("fullscreen") ? PlayerPrefs.GetInt("fullscreen") == 1 : false;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("quality", qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("volume", volume);
    }
}
