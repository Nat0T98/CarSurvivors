using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayButton()
    { 
        AudioManager.GlobalAudioManager.PlaySFX("Play Button");
        Invoke("StartDelay", 2);

        //SceneManager.LoadScene("Level 1");
    }

    public void GaragaeButton()
    {
        //SceneManager.LoadScene("Garage");
        AudioManager.GlobalAudioManager.PlaySFX("UI Button");

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

    public void StartDelay()
    {
        SceneManager.LoadScene("Level 1");
    }
}
