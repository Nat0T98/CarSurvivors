using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public static bool isGameOver = false;
    public GameObject GameOverCanvas;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float elapsedTime;

    private void Start()
    {
        Time.timeScale = 1.0f;
    }
       

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);    
    }


    public void GameOverScreen()
    {
        SFX_Manager.GlobalSFXManager.StopCarSFX();
        SFX_Manager.GlobalSFXManager.driftSource.enabled = false;
        GameOverCanvas.SetActive(true);
        Time.timeScale = 0f;
        //isGameOver = true;
    }

    public void PlayAgainButton()
    {
        GameManager.Instance.upgradePointsRef = 0;
        SFX_Manager.GlobalSFXManager.PlaySFX("UI_Button");
        SFX_Manager.GlobalSFXManager.driftSource.enabled = true;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }


    public void MainMenuButton()
    {
        GameManager.Instance.upgradePointsRef = 0;
        SFX_Manager.GlobalSFXManager.PlaySFX("UI_Button");
        GameOverCanvas.SetActive(false);
        Time.timeScale = 1f;
        MusicManager.GlobalMusicManager.StopMusic();
        SceneManager.LoadSceneAsync("Main Menu");
        SFX_Manager.GlobalSFXManager.driftSource.enabled = true;
    }


    //public void LoadMainMenu()
    //{
    //    AudioManager.GlobalAudioManager.StopMusic();
    //    //SceneManager.LoadScene("Main Menu");
    //}
}
