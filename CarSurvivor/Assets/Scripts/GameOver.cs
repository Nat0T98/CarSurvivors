using System.Collections;
using System.Collections.Generic;
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
        GameOverCanvas.SetActive(true);
        Time.timeScale = 0f;
        //isGameOver = true;
    }

    public void PlayAgainButton()
    {
        //AudioManager.GlobalAudioManager.PlaySFX("UI Button");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }


    public void MainMenuButton()
    {
        //AudioManager.GlobalAudioManager.PlaySFX("UI Button");
        GameOverCanvas.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }


    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
