using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeRushTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    public float KillBonus;
    private bool isOutOfTime;
    public GameOver GameOver;

    // Update is called once per frame
    void Update()
    {

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            isOutOfTime = false;
        }
        else if (remainingTime < 0)
        {
            remainingTime = 0;
            //isOutOfTime = true;
            GameOver.GameOverScreen();
            //Add logic here for end of round 
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        /*if (Input.GetKeyDown(KeyCode.T))
        {
            AddTime(KillBonus);
        }*/

    }

    public void AddTime(float killBonus)
    {
        remainingTime += killBonus;
        Debug.Log("Time increased by " + killBonus + " seconds.");
    }


    void OutOfTime()
    {
        if(isOutOfTime == true)
        {
             GameOver.GameOverScreen();
        }
       
    }
}
