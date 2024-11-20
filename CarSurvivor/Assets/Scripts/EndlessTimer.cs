using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndlessTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float elapsedTime;
    [SerializeField] TextMeshProUGUI upgradeText;
    [SerializeField] private TextMeshProUGUI upgradePromptText;

    private float flashTimer = 0f;
    private const float flashDuration = 0.5f;
    private bool isUpgradeFlashing = false;
   
    private void Start()
    {
        MusicManager.GlobalMusicManager.PlayGameMusic();
        upgradePromptText.gameObject.SetActive(false);
        //MusicManager.GlobalMusicManager.LoopMusic("Game_Music1");
      
    }


    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (GameManager.Instance != null)
        {
            int upgradePoints = GameManager.Instance.upgradePointsRef;
            upgradeText.text = string.Format("Upgrade Points: {0}", upgradePoints);

            if (upgradePoints >= 5)
            {
                upgradePromptText.gameObject.SetActive(true);
                if (!isUpgradeFlashing)
                {
                    isUpgradeFlashing = true;
                    flashTimer = 0f;
                }
                UpgradePromptFlash();
            }
            else
            {
                isUpgradeFlashing = false;
                upgradePromptText.gameObject.SetActive(false);
            }
        }

    }



    private void UpgradePromptFlash()
    {
        flashTimer += Time.deltaTime;

        if (flashTimer >= flashDuration)
        {
            flashTimer = 0f;
        }
        float t = Mathf.PingPong(flashTimer * 2f / flashDuration, 1f);
        upgradePromptText.color = Color.Lerp(Color.white, Color.yellow, t); // Flash between white and yellow
    }

}
