using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndlessTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float elapsedTime;
    [SerializeField] TextMeshProUGUI upgradeText;


    private void Start()
    {
        AudioManager.GlobalAudioManager.LoopMusic("Endless Music");
    }


    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        upgradeText.text = string.Format("Upgrade Points: {0}", GameManager.Instance.upgradePointsRef);

        

        //Countdown Timer
        /*if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime < 0)
        {
            remainingTime = 0;
            //Add logic here for end of round 
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);*/

    }

}
