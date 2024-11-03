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

   
    public List<string> GameMusicPlaylist = new List<string>();

    private List<AudioClip> currentPlaylist = new List<AudioClip>();
    private int currentIndex = 0;

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

    public void PlaySFX(string clipName)
    {
        if (Music_Lib.ContainsKey(clipName))
        {
            AudioSource theSFX = Instantiate(Music_Prefab).GetComponent<AudioSource>();
            theSFX.PlayOneShot(Music_Lib[clipName]);
            Destroy(theSFX.gameObject, Music_Lib[clipName].length);
        }
    }

    public void LoopMusic(string clipName)
    {
        if (Music_Lib.ContainsKey(clipName))
        {
            MusicSource.clip = Music_Lib[clipName];
            MusicSource.loop = true;
            MusicSource.Play();
        }
    }

    public void StopMusic()
    {
        MusicSource.Stop();
    }

    public void PlayGameMusic()
    {
        currentPlaylist.Clear();
        foreach (string musicClip in GameMusicPlaylist)
        {
            if (Music_Lib.ContainsKey(musicClip))
            {
                currentPlaylist.Add(Music_Lib[musicClip]);
            }
            else
            {
                Debug.LogWarning("Clip '{musicClip}' not found");
            }
        }

        /*if (currentPlaylist.Count == 0)
        {
            Debug.LogWarning("No valid songs found for the playlist.");
            return;
        }*/

        ShuffleList(currentPlaylist);
        currentIndex = 0;
        PlayNextSong();
    }

    private void PlayNextSong()
    {
        if (currentIndex >= currentPlaylist.Count)
        {
            ShuffleList(currentPlaylist);
            currentIndex = 0;
        }

        //play the next song
        MusicSource.clip = currentPlaylist[currentIndex];
        MusicSource.loop = false;
        MusicSource.Play();
        currentIndex++;
        StartCoroutine(WaitForSongToEnd());
    }

    private IEnumerator WaitForSongToEnd()
    {
        yield return new WaitWhile(() => MusicSource.isPlaying);
        PlayNextSong();
    }

    private void ShuffleList(List<AudioClip> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            AudioClip temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
