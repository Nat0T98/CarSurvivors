using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPS_Counter : MonoBehaviour
{
    public float FPS;
    public TextMeshProUGUI timerText;
    // Update is called once per frame
    void Update()
    {
        float current = 0;
        current = Time.frameCount/Time.time;
        current = (int)(1f/Time.unscaledDeltaTime);
        timerText.text = current.ToString();
    }


}
