using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public List<string> ClipNames = new List<string>();
    public List<AudioClip> ClipList = new List<AudioClip>();
    private Dictionary<string, AudioClip> Music_Lib = new Dictionary<string, AudioClip>();


    public GameObject Music_Prefab;
    public AudioSource MusicSource;
    public static MusicManager GlobalMusicManager;

    private void Awake()
    {
        for (int i = 0; i < ClipNames.Count; i++)
            Music_Lib.Add(ClipNames[i], ClipList[i]);
        if (GlobalMusicManager != null)
        {
            Destroy(gameObject);
        }
        else
        {
            GlobalMusicManager = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySFX(string CLipName)
    {
        if(Music_Lib.ContainsKey(CLipName)) 
        {
            AudioSource TheSFX = Instantiate(Music_Prefab).GetComponent<AudioSource>();
            TheSFX.PlayOneShot(Music_Lib[CLipName]); //sets clip and plays it
            Destroy(TheSFX.gameObject, Music_Lib[CLipName].length);
        }
    }

    public void LoopMusic(string clipName)
    {
        if (Music_Lib.ContainsKey(clipName))
        {
            MusicSource.clip = Music_Lib[clipName];
            MusicSource.loop = true; // Enable looping
            MusicSource.Play(); // Play the music
        }
    }

    public void StopMusic()
    {
        MusicSource.Stop(); // Stop the music
    }

}
