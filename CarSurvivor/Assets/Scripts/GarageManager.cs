using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GarageManager : MonoBehaviour
{
    private void Awake()
    {
        AudioManager.GlobalAudioManager.LoopMusic("Garage Music");
    }

    public void BackButton()
    {
        AudioManager.GlobalAudioManager.PlaySFX("UI Button");
        Invoke("LoadMainMenu", 1);
    }

    public void LoadMainMenu()
    {
        AudioManager.GlobalAudioManager.StopMusic();
        SceneManager.LoadSceneAsync("Main Menu");
    }
}
