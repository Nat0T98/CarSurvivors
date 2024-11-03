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
        MusicManager.GlobalMusicManager.PlayGameMusic();
        //MusicManager.GlobalMusicManager.LoopMusic("Game_Music1");
    }


    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        upgradeText.text = string.Format("Upgrade Points: {0}", GameManager.Instance.upgradePointsRef);

    }

}
