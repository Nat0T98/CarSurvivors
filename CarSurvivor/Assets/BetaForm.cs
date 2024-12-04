using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BetaForm : MonoBehaviour
{
    public void OpenURL()
    {
        Application.OpenURL("https://forms.gle/u1CWkfEo2p3uqecu7");
        Debug.Log("is this working?");
    }

}


