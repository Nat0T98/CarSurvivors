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
        AudioManager.GlobalAudioManager.PlaySFX("UI Button");
    } 


    public void MainMenuButton()
    { 
        AudioManager.GlobalAudioManager.PlaySFX("UI Button");
        PauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Invoke("LoadMainMenu", 1);
              
    }


    public void LoadMainMenu()
    {
        AudioManager.GlobalAudioManager.StopMusic();
        SceneManager.LoadScene("Main Menu");
    }
    

    

}
