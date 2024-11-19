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
    public AudioSource drivingAudioSource;
    public AudioSource driftAudioSource;
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


    public void PlayDrivingSFX(string sfxName, bool shouldPlay, float pitch = 1f, float volume = 1f) 
    {
        
        if (!SFX_Lib.ContainsKey(sfxName))
        {
            Debug.LogWarning($"SFX '{sfxName}' not found");
            return;
        }

        AudioClip drivingClip = SFX_Lib[sfxName];

        if (shouldPlay)
        {
            if (!drivingAudioSource.isPlaying || drivingAudioSource.clip != drivingClip)
            {
                drivingAudioSource.clip = drivingClip;
                drivingAudioSource.pitch = pitch;
                drivingAudioSource.volume = Mathf.Clamp(volume, 0f, 1f);
                drivingAudioSource.loop = true;
                drivingAudioSource.Play();
            }
        }
        else
        {
            if (drivingAudioSource.isPlaying)
            {
                drivingAudioSource.Stop();
            }
        }
    }


    public void PlayDriftSFX(string sfxName, bool shouldPlay, float volume = 1f)
    {

        if (!SFX_Lib.ContainsKey(sfxName))
        {
            Debug.LogWarning($"SFX '{sfxName}' not found");
            return;
        }

        AudioClip driftClip = SFX_Lib[sfxName];

        if (shouldPlay)
        {
            if (!driftAudioSource.isPlaying || driftAudioSource.clip != driftClip)
            {
                driftAudioSource.clip = driftClip;
                driftAudioSource.volume = Mathf.Clamp(volume, 0f, 1f);
                driftAudioSource.loop = true;
                driftAudioSource.Play();
            }
        }
        else
        {
            if (driftAudioSource.isPlaying)
            {
                driftAudioSource.Stop();
            }
        }
    }

    public bool IsDriftingSFXPlaying()
    {
        return driftAudioSource.isPlaying;
    }
}
