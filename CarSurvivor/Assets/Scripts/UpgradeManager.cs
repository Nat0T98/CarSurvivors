using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GameObject UpgradeCanvas;
    PauseMenu pauseMenu;
    public static bool isMenuActive = false;
    public MainCar Car;


    public int UpgradePoints = 0;
    public int PointsToUpgrade = 5;
    public int EnemyPointWorth = 1;

    private void Start()
    {
        Time.timeScale = 1.0f;
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.U))
        {
            if (isMenuActive)
            {
                CloseMenu();
            }
            else
            {
                OpenUpgradeMenu();
            }
        }

        CheckLevelUp();
    }

    public void AddUpgradePoints()
    {
        UpgradePoints += EnemyPointWorth;
    }
    public void CheckLevelUp()
    {
        if (UpgradePoints >= PointsToUpgrade)
        {
            OpenUpgradeMenu();            
        }
    }

    public void OpenUpgradeMenu()
    {
        UpgradeCanvas.SetActive(true);
        Time.timeScale = 0f;
        isMenuActive = true;
    }


    public void CloseMenu()
    {
        UpgradeCanvas.SetActive(false);
        Time.timeScale = 1f;
        isMenuActive = false;
        AudioManager.GlobalAudioManager.PlaySFX("UI Button");
        //Debug.Log(UpgradeCanvas.active);
    }

    public void UpgradeSelected()
    {
        UpgradeCanvas.SetActive(false);
        Time.timeScale = 1f;
        isMenuActive = false;
        UpgradePoints = 0;  
        //AudioManager.GlobalAudioManager.PlaySFX("UI Button");
    }
    public void ChooseUpgrade1()
    {
        Debug.Log("Upgrade 1");
        
        UpgradeSelected();
    }


    public void ChooseUpgrade2()
    {
        Debug.Log("Upgrade 2");
        Car.MelleUpgradeTest();
        UpgradeSelected();
    }


    public void ChooseUpgrade3()
    {
        Debug.Log("Upgrade 3");
        Car.BoostUpgrade();
        UpgradeSelected();
    }
}
