using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject PauseMenuCanvas;
    
    public Canvas SettingsCanvas;
    public Canvas PauseCanvas;
    public Canvas UiCanvas;

    private void Start()
    {
        
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPaused) 
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Pause() 
    {
        PauseMenuCanvas.SetActive(true);
        UiCanvas.gameObject.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
        SFX_Manager.GlobalSFXManager.StopCarSFX();
        SFX_Manager.GlobalSFXManager.driftSource.enabled = false;
    }

    public void Resume()
    {
        PauseMenuCanvas.SetActive(false);
        UiCanvas.gameObject.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
        SFX_Manager.GlobalSFXManager.PlaySFX("UI_Button");
        SFX_Manager.GlobalSFXManager.driftSource.enabled = true;
    } 


    public void MainMenuButton()
    { 
        SFX_Manager.GlobalSFXManager.PlaySFX("UI_Button");
        PauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Invoke("LoadMainMenu", 1);
        SFX_Manager.GlobalSFXManager.driftSource.enabled = true;
    }


    public void LoadMainMenu()
    {
        MusicManager.GlobalMusicManager.StopMusic();
        SceneManager.LoadSceneAsync("Main Menu");
    }

    public void SettingsButton()
    {
        SFX_Manager.GlobalSFXManager.PlaySFX("UI_Button");
        PauseCanvas.gameObject.SetActive(false);
        SettingsCanvas.gameObject.SetActive(true);
    }

    public void ShowSettings()
    {
        PauseMenuCanvas.SetActive(false);
        SettingsCanvas.gameObject.SetActive(true);
    }

}
