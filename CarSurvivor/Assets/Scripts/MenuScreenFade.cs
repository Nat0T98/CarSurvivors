using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreenFade : MonoBehaviour
{
    public Image fadeImage;
    private Color color;
    public float fadeDuration;
    private float time;
    private float value = 1;
    public GameObject fadeCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        //fadeCanvas.SetActive(true);
        color = fadeImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (value > 0)
        {
            time += Time.deltaTime;
            value = Mathf.Lerp(1f, 0f, time / fadeDuration);
            color.a = value;
            fadeImage.color = color;
        }
        else
        {
            fadeCanvas.SetActive(false);
        }
    }
}
