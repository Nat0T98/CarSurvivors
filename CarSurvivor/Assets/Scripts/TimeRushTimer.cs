using TMPro;
using UnityEngine;

public class TimeRushTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private TextMeshProUGUI upgradePromptText;
    [SerializeField] private TextMeshProUGUI plusTimePrompt;
    [SerializeField] private float remainingTime;
    public float killBonus;
    public GameOver GameOver;


    private bool isUpgradeFlashing = false;
    private float upgradeFlashTimer = 0f;
    private float flashTimer = 0f; 
    private const float flashDuration = 0.5f;

    private bool isGreenFlash = false; 
    private float greenFlashTimer = 0f; 
    private const float greenFlashDuration = 0.6f;
    private void Start()
    {
        if (GameOver == null)
        {
            Debug.LogError("GameOver reference is not set in TimeRushTimer.");
        }
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is not available.");
        }

        MusicManager.GlobalMusicManager.PlayGameMusic();
        upgradePromptText.gameObject.SetActive(false);
        plusTimePrompt.gameObject.SetActive(false);

    }

    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            remainingTime = 0;
            GameOver.GameOverScreen();
        }

     
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (isGreenFlash)
        {
            AddTimeFlash();
            AddTimePrompt();
        }
        // Flashing Text Effect for Low Time
        else if (remainingTime < 15)
        {
            LowTimeFlash();
        }
        else
        {
            timerText.color = Color.white;
            plusTimePrompt.color = Color.white;
        }


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


    public void AddTime()
    {
        remainingTime += killBonus;
        isGreenFlash = true;
        greenFlashTimer = 0f;
    }

    private void LowTimeFlash()
    {
        flashTimer += Time.deltaTime;

        if (flashTimer >= flashDuration)
        {
            flashTimer = 0f;
        }

        // Lerp color to create a flashing effect
        float t = Mathf.PingPong(flashTimer * 2f / flashDuration, 1f);
        timerText.color = Color.Lerp(Color.red, new Color(1, 0, 0, 0.5f), t);
    }

    private void AddTimeFlash()
    {
        greenFlashTimer += Time.deltaTime;
        timerText.color = Color.green;

        if (greenFlashTimer >= greenFlashDuration)
        {
            isGreenFlash = false;
        }
    }


    private void AddTimePrompt()
    {
        plusTimePrompt.gameObject.SetActive(true);
        greenFlashTimer += Time.deltaTime;
        plusTimePrompt.color = Color.green;

        if (greenFlashTimer >= greenFlashDuration)
        {
            isGreenFlash = false;
            plusTimePrompt.gameObject.SetActive(false);
        }
    }


    private void UpgradePromptFlash()
    {
        upgradeFlashTimer += Time.deltaTime;

        if (upgradeFlashTimer >= flashDuration)
        {
            upgradeFlashTimer = 0f;
        }
        float t = Mathf.PingPong(upgradeFlashTimer * 2f / flashDuration, 1f);
        upgradePromptText.color = Color.Lerp(Color.white, Color.yellow, t); // Flash between white and yellow
    }
}
