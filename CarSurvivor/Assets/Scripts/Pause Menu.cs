using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject PauseMenuCanvas;


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
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        PauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        SFX_Manager.GlobalSFXManager.PlaySFX("UI_Button");
    } 


    public void MainMenuButton()
    { 
        SFX_Manager.GlobalSFXManager.PlaySFX("UI_Button");
        PauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Invoke("LoadMainMenu", 1);
              
    }


    public void LoadMainMenu()
    {
        MusicManager.GlobalMusicManager.StopMusic();
        SceneManager.LoadSceneAsync("Main Menu");
    }
    

    

}
