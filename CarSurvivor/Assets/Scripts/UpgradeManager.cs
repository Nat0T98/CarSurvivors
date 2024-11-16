using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GameObject UpgradeCanvas;
    PauseMenu pauseMenu;
    public static bool isMenuActive = false;
    public CarPlayer Car;


    public int UpgradePoints = 0;
    public int PointsToUpgrade = 5;
    public int EnemyPointWorth = 1;

    private bool canOpenMenu;

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
            else if (canOpenMenu)
            {
                OpenUpgradeMenu();
            }
        }

        CheckLevelUp();
    }

    public void AddUpgradePoints()
    {
        UpgradePoints += EnemyPointWorth;
        GameManager.Instance.upgradePointsRef = UpgradePoints;
    }
    public void CheckLevelUp()
    {
        if (UpgradePoints >= PointsToUpgrade)
        {
            canOpenMenu = true;           
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
        SFX_Manager.GlobalSFXManager.PlaySFX("UI_Button");
        //Debug.Log(UpgradeCanvas.active);
    }

    public void UpgradeSelected()
    {
        UpgradePoints -= PointsToUpgrade;
        GameManager.Instance.upgradePointsRef = UpgradePoints;
        Car.health += 5;
        if (UpgradePoints < PointsToUpgrade)
        {
            UpgradeCanvas.SetActive(false);
            canOpenMenu = false;
            Time.timeScale = 1f;
            isMenuActive = false;
        }
    }
    public void ChooseUpgrade1()
    {
        Debug.Log("Upgrade 1");
        Car.CarMiniGunUpgrade();
        SFX_Manager.GlobalSFXManager.PlaySFX("Upgrade_Button", 0.5f);
        UpgradeSelected();
    }


    public void ChooseUpgrade2()
    {
        Debug.Log("Upgrade 2");
        Car.MelleUpgradeTest();
        SFX_Manager.GlobalSFXManager.PlaySFX("Upgrade_Button", 0.5f);
        UpgradeSelected();
    }


    public void ChooseUpgrade3()
    {
        Debug.Log("Upgrade 3");
        Car.BoostUpgrade();
        SFX_Manager.GlobalSFXManager.PlaySFX("Upgrade_Button", 0.5f);
        UpgradeSelected();
    }
}
