using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("UI Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Drop Downs")]
    public TMPro.TMP_Dropdown ResDropDown;
    public TMPro.TMP_Dropdown graphicsDropdown;
    public Toggle fullscreenToggle;

    [Header("Canvas Ref")]
    public Canvas MenuCanvas;
    public Canvas SettingsCanvas;

    Resolution[] resolutions;

    private void Start()
    {
        //Resolution Initialiaztion
        resolutions = Screen.resolutions;

        ResDropDown.ClearOptions();
        List<string> Options = new List<string>();

        int currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            Options.Add(option);
            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height)
            {
                currentResIndex = i;
            }
        }
        ResDropDown.AddOptions(Options);

        //Loading Player Prefs
        int savedResIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResIndex);
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        int qualityLevel = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());

        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        //Resolution
        ResDropDown.value = savedResIndex;
        ResDropDown.RefreshShownValue();
        //Screen Mode
        fullscreenToggle.isOn = isFullscreen;
        Screen.fullScreen = isFullscreen;
        //Graphics Mode
        graphicsDropdown.value = qualityLevel;
        QualitySettings.SetQualityLevel(qualityLevel);
        //Audio Settings
        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        // Add Listeners for Changes
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        ResDropDown.onValueChanged.AddListener(SetResolution);
        fullscreenToggle.onValueChanged.AddListener(SetFullScreen);
        graphicsDropdown.onValueChanged.AddListener(SetGraphicsQuality);
    }

    public void SetMasterVolume(float value)
    {
        if (value <= 0.0001f) // If slider is at the bottom
        {
            audioMixer.SetFloat("MasterVolume", -80f); // Mute audio
        }
        else
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20); // Adjust volume
        }
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        if (value <= 0.0001f)
        {
            audioMixer.SetFloat("MusicVolume", -80f); 
        }
        else
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20); 
        }
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        if (value <= 0.0001f)
        { 
            audioMixer.SetFloat("SFXVolume", -80f);
        }
        else
        {
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
            PlayerPrefs.SetFloat("SFXVolume", value);
        }
    }

    public void SetGraphicsQuality(int QualityIndex)
    {
        QualitySettings.SetQualityLevel(QualityIndex);
        PlayerPrefs.SetInt("QualityLevel", QualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("Fullscreen", isFullScreen ? 1 : 0);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
    }

   

    public void ReturnButton()
    {
        SFX_Manager.GlobalSFXManager.PlaySFX("UI_Button");
        MenuCanvas.gameObject.SetActive(true);
        SettingsCanvas.gameObject.SetActive(false);
    }

}
