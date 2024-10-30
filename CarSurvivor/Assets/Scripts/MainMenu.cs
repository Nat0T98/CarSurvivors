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
       MusicManager.GlobalMusicManager.LoopMusic("Menu Music");
    }
    
    public void EndlessModeButton()
    {
        SFX_Manager.GlobalSFXManager.PlaySFX("Play_Button");
        Invoke("LoadEndlessMode", 2); //wait 2 seconds so that SFX can play
    }
    public void TimeRushModeButton()
    {
        MusicManager.GlobalMusicManager.PlaySFX("Play Button");
        Invoke("LoadTimeRushMode", 2); //wait 2 seconds so that SFX can play
    }


    public void GaragaeButton()
    {
        SFX_Manager.GlobalSFXManager.PlaySFX("UI_Button");
        Invoke("LoadGarage", 1);

    }

    public void SettingsButton()
    {

        SFX_Manager.GlobalSFXManager.PlaySFX("UI_Button");

    }

    public void QuitButton()
    {
        //Application.Quit();
        Debug.Log("Quit");
        SFX_Manager.GlobalSFXManager.PlaySFX("UI_Button");
    }

    public void LoadEndlessMode()
    {
        MusicManager.GlobalMusicManager.StopMusic();
        SceneManager.LoadSceneAsync("Showcase");
    }
    public void LoadTimeRushMode()
    {
        MusicManager.GlobalMusicManager.StopMusic();
        SceneManager.LoadSceneAsync("TimeRushTest");
    }


    public void LoadGarage()
    {
        MusicManager.GlobalMusicManager.StopMusic();
        SceneManager.LoadSceneAsync("Garage");
    }
}
