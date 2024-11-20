using UnityEngine;
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
        SFX_Manager.GlobalSFXManager.PlaySFX("Play");
        Invoke("LoadEndlessMode", 2); //wait 2 seconds so that SFX can play
    }
    public void TimeRushModeButton()
    {
        MusicManager.GlobalMusicManager.PlaySFX("Play");
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
        Application.Quit();
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
        SceneManager.LoadSceneAsync("Time Rush");
    }


    public void LoadGarage()
    {
        MusicManager.GlobalMusicManager.StopMusic();
        SceneManager.LoadSceneAsync("Garage");
    }
}
