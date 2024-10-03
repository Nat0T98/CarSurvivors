using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("Level 1");

    }

    public void GaragaeButton()
    {
        SceneManager.LoadScene("Garage");
    }

    public void QuitButton()
    {
        //Application.Quit();
        Debug.Log("Quit");
    }
}
