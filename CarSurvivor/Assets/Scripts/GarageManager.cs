using UnityEngine;
using UnityEngine.SceneManagement;


public class GarageManager : MonoBehaviour
{
    private void Awake()
    {
        MusicManager.GlobalMusicManager.LoopMusic("Garage_Music");
    }

    public void BackButton()
    {
        SFX_Manager.GlobalSFXManager.PlaySFX("UI_Button");
        Invoke("LoadMainMenu", 1);
    }

    public void LoadMainMenu()
    {
        MusicManager.GlobalMusicManager.StopMusic();
        SceneManager.LoadSceneAsync("Main Menu");
    }
}
