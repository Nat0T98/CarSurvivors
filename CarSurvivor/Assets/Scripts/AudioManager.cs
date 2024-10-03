using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<string> ClipNames = new List<string>();
    public List<AudioClip> ClipList = new List<AudioClip>();
    private Dictionary<string, AudioClip> SFX_Lib = new Dictionary<string, AudioClip>();


    public GameObject SFX_Prefab;

    public static AudioManager GlobalAudioManager;

    private void Start()
    {
        GlobalAudioManager = this;

        for (int i = 0; i < ClipNames.Count; i++)
            SFX_Lib.Add(ClipNames[i], ClipList[i]);
    }


    public void PlaySFX(string CLipName)
    {
        if(SFX_Lib.ContainsKey(CLipName)) 
        {
            AudioSource TheSFX = Instantiate(SFX_Prefab).GetComponent<AudioSource>();
            TheSFX.PlayOneShot(SFX_Lib[CLipName]); //sets clip and plays it
            Destroy(TheSFX.gameObject, SFX_Lib[CLipName].length);
        }
    }
}
