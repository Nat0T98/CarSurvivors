using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Canvas MainMenuCanvas;
    public Canvas SettingsCanvas;
    public GameObject cam;
    private MenuCam menuCamScript;

    private void Awake()
    {
        
    }

    private void Start()
    {
       MusicManager.GlobalMusicManager.LoopMusic("Menu Music");
       ShowMainMenu();
       menuCamScript = cam.GetComponent<MenuCam>();
    }
    private void Update()
    {
        
    }
    public void EndlessModeButton()
    {
        SFX_Manager.GlobalSFXManager.PlaySFX("Play");
        StartCoroutine(menuCamScript.EndMove());
        Invoke("LoadEndlessMode", 5); //wait 2 seconds so that SFX can play
    }
    public void TimeRushModeButton()
    {
        MusicManager.GlobalMusicManager.PlaySFX("Play");
        StartCoroutine(menuCamScript.EndMove());
        Invoke("LoadTimeRushMode", 5); //wait 2 seconds so that SFX can play
    }


    public void GaragaeButton()
    {
        SFX_Manager.GlobalSFXManager.PlaySFX("UI_Button");
        Invoke("LoadGarage", 1);

    }

    public void SettingsButton()
    {
        
        SFX_Manager.GlobalSFXManager.PlaySFX("UI_Button");
        ShowSettings();
    }

    public void QuitButton()
    { 
        SFX_Manager.GlobalSFXManager.PlaySFX("UI_Button");
        Application.Quit();
        Debug.Log("Quit");
       
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


    public void ShowMainMenu()
    {
        MainMenuCanvas.gameObject.SetActive(true);
        SettingsCanvas.gameObject.SetActive(false);
    }

    public void ShowSettings()
    {
        MainMenuCanvas.gameObject.SetActive(false);
        SettingsCanvas.gameObject.SetActive(true);
    }

   



    /*
        public void LoadGarage()
        {
            MusicManager.GlobalMusicManager.StopMusic();
            SceneManager.LoadSceneAsync("Garage");
        }*/
}
