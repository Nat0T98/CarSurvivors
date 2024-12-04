using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{

    [Header("Canvas Ref")]
    public Canvas CreditsCanvas;
    public Canvas SettingsCanvas;



    public void ReturnButton()
    {
        SFX_Manager.GlobalSFXManager.PlaySFX("UI_Button");
        SettingsCanvas.gameObject.SetActive(true);
        CreditsCanvas.gameObject.SetActive(false);
    }
}
