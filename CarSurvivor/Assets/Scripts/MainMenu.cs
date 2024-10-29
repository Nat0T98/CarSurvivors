using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        
    }

    private void Start()
    {
       AudioManager.GlobalAudioManager.LoopMusic("Menu Music");
    }
    
    public void EndlessModeButton()
    { 
        AudioManager.GlobalAudioManager.PlaySFX("Play Button");
        Invoke("LoadEndlessMode", 2); //wait 2 seconds so that SFX can play
    }
    public void TimeRushModeButton()
    {
        AudioManager.GlobalAudioManager.PlaySFX("Play Button");
        Invoke("LoadTimeRushMode", 2); //wait 2 seconds so that SFX can play
    }


    public void GaragaeButton()
    {
        AudioManager.GlobalAudioManager.PlaySFX("UI Button");
        Invoke("LoadGarage", 1);

    }

    public void SettingsButton()
    {

        AudioManager.GlobalAudioManager.PlaySFX("UI Button");

    }

    public void QuitButton()
    {
        //Application.Quit();
        Debug.Log("Quit");
        AudioManager.GlobalAudioManager.PlaySFX("UI Button");
    }

    public void LoadEndlessMode()
    {
        AudioManager.GlobalAudioManager.StopMusic();
        SceneManager.LoadSceneAsync("Showcase");
    }
    public void LoadTimeRushMode()
    {
        AudioManager.GlobalAudioManager.StopMusic();
        SceneManager.LoadSceneAsync("TimeRushTest");
    }


    public void LoadGarage()
    {
        AudioManager.GlobalAudioManager.StopMusic();
        SceneManager.LoadSceneAsync("Garage");
    }
}
