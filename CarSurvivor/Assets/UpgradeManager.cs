using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GameObject UpgradeCanvas;
    PauseMenu pauseMenu;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void CloseButton()
    {
        UpgradeCanvas.SetActive(false);
        pauseMenu.Resume();
    }
}
