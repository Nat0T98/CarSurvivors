using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Manager : MonoBehaviour
{
    public List<string> ClipNames = new List<string>();
    public List<AudioClip> ClipList = new List<AudioClip>();
    private Dictionary<string, AudioClip> SFX_Lib = new Dictionary<string, AudioClip>();


    public GameObject SFX_Prefab;
    public AudioSource MusicSource;
    public static SFX_Manager GlobalSFXManager;

    private void Awake()
    {
        for (int i = 0; i < ClipNames.Count; i++)
            SFX_Lib.Add(ClipNames[i], ClipList[i]);
        if (GlobalSFXManager != null)
        {
            Destroy(gameObject);
        }
        else
        {
            GlobalSFXManager = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySFX(string CLipName)
    {
        if (SFX_Lib.ContainsKey(CLipName))
        {
            AudioSource TheSFX = Instantiate(SFX_Prefab).GetComponent<AudioSource>();
            TheSFX.PlayOneShot(SFX_Lib[CLipName]); //sets clip and plays it
            Destroy(TheSFX.gameObject, SFX_Lib[CLipName].length);
        }
    }

    public void LoopMusic(string clipName)
    {
        if (SFX_Lib.ContainsKey(clipName))
        {
            MusicSource.clip = SFX_Lib[clipName];
            MusicSource.loop = true; // Enable looping
            MusicSource.Play(); // Play the music
        }
    }

    public void StopMusic()
    {
        MusicSource.Stop(); // Stop the music
    }

}
