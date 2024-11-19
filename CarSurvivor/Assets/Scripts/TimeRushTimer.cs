using TMPro;
using UnityEngine;

public class TimeRushTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private float remainingTime;
    public float killBonus;
    public GameOver GameOver;

    private bool isFlashing = false; // Tracks whether the timer is in flashing mode for low time
    private float flashTimer = 0f; // Timer for controlling the flashing effect
    private const float flashDuration = 0.5f; // Duration for each flash cycle

    private bool isGreenFlash = false; // Tracks if the text is in green flash mode
    private float greenFlashTimer = 0f; // Timer for controlling the green flash
    private const float greenFlashDuration = 0.3f; // Duration for green flash

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
    }

    private void Update()
    {
        // Countdown Timer Logic
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            remainingTime = 0;
            GameOver.GameOverScreen(); // Trigger game over if time is up
        }

        // Format and Update Timer Text
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Handle Green Flash
        if (isGreenFlash)
        {
            AddTimeFlash();
        }
        // Flashing Text Effect for Low Time
        else if (remainingTime < 15)
        {
            LowTimeFlash();
        }
        else
        {
            timerText.color = Color.white;
            isFlashing = false;
        }

        
        if (GameManager.Instance != null)
        {
            upgradeText.text = string.Format("Upgrade Points: {0}", GameManager.Instance.upgradePointsRef);
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
        isFlashing = true;
        flashTimer += Time.deltaTime;

        // Alternate between red and transparent colors based on the flash duration
        if (flashTimer >= flashDuration)
        {
            flashTimer = 0f; // Reset the flash timer
        }

        // Lerp color to create a flashing effect
        float t = Mathf.PingPong(flashTimer * 2f / flashDuration, 1f);
        timerText.color = Color.Lerp(Color.red, new Color(1, 0, 0, 0.5f), t); // Flash between red and semi-transparent red
    }

    private void AddTimeFlash()
    {
        greenFlashTimer += Time.deltaTime;

        // Set the timer text to green
        timerText.color = Color.green;

        // Reset after the green flash duration
        if (greenFlashTimer >= greenFlashDuration)
        {
            isGreenFlash = false;
        }
    }
}
