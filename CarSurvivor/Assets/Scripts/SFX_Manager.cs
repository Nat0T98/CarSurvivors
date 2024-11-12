using System.Collections.Generic;
using UnityEngine;

public class SFX_Manager : MonoBehaviour
{
    public List<string> ClipNames = new List<string>();
    public List<AudioClip> ClipList = new List<AudioClip>();
    private Dictionary<string, AudioClip> SFX_Lib = new Dictionary<string, AudioClip>();

    public GameObject SFX_Prefab;
    public AudioSource MusicSource;
    private AudioSource boostSource; 
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

        //dedicated source for boost sfx
        boostSource = gameObject.AddComponent<AudioSource>();
        boostSource.loop = true;
    }

    public void PlaySFX(string clipName, float volume = 1.0f) 
    {
        if (SFX_Lib.ContainsKey(clipName))
        {
            AudioSource TheSFX = Instantiate(SFX_Prefab).GetComponent<AudioSource>();
            TheSFX.volume = volume;
            TheSFX.PlayOneShot(SFX_Lib[clipName]);
            Destroy(TheSFX.gameObject, SFX_Lib[clipName].length);
        }
    }

    public void PlayBoostSFX(float volume = 1.0f) 
    {
        if (SFX_Lib.ContainsKey("Boost") && !boostSource.isPlaying)
        {
            boostSource.clip = SFX_Lib["Boost"];
            boostSource.volume = volume; 
            boostSource.Play();
        }
    }
    public void StopBoostSFX()
    {
        if (boostSource.isPlaying)
        {
            boostSource.Stop(); // Stop playing the boost sound
        }
    }

    public void LoopMusic(string clipName)
    {
        if (SFX_Lib.ContainsKey(clipName))
        {
            MusicSource.clip = SFX_Lib[clipName];
            MusicSource.loop = true; // Enable looping
            MusicSource.Play();
        }
    }

    public void StopMusic()
    {
        MusicSource.Stop(); // Stop the music
    }
}
