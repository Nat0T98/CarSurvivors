using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer MasterMixer;
    public AudioMixerGroup MusicMixer;
    public AudioMixerGroup SFXMixer;

    public TMPro.TMP_Dropdown ResDropDown;
    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        ResDropDown.ClearOptions();
        List<string> Options = new List<string>();


        int currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            Options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }
        ResDropDown.AddOptions(Options);
        ResDropDown.value = currentResIndex;
        ResDropDown.RefreshShownValue();
    }

    public void SetMasterVolume(float volume)
    {
        MasterMixer.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        MasterMixer.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        MasterMixer.SetFloat("SFXVolume", volume);
    }

    public void SetGraphicsQuality(int QualityIndex)
    {
        QualitySettings.SetQualityLevel(QualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
       
        Screen.fullScreen = isFullScreen;

    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

}
