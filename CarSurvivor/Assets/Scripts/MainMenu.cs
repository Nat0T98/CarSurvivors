using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayButton()
    { 
        AudioManager.GlobalAudioManager.PlaySFX("Play Button");
        Invoke("LoadLevel1", 2); //wait 2 seconds so that SFX can play
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

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level 1");
    }


    public void LoadGarage()
    {
        SceneManager.LoadScene("Garage");
    }
}
