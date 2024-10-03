using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GarageManager : MonoBehaviour
{
    public void BackButton()
    {
        AudioManager.GlobalAudioManager.PlaySFX("UI Button");
        Invoke("LoadMainMenu", 1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
