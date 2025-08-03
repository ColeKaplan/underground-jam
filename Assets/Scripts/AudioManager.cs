using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSource;
    public AudioClip musicClip;

    private Dictionary<string, AudioSource> clipSourceMap = new Dictionary<string, AudioSource>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (musicSource != null && musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    /// <summary>
    /// To use, attach an AudioClip to your GameObject and call PlaySFX(clip) whenever you want to play the Sfx. 
    /// Will create a new source and play the sound each time, so do not call repeatedly unless you want sound gore.
    /// </summary>
    /// <param name="clip">Clip to play</param>
    /// <param name="loop">Whether to play once or repeatedly until calling StopSFX(clip)</param>
    public void PlaySFX(AudioClip clip, bool loop = false)
    {
        if (clip == null)
        {
            return;
        }
        if (!loop)
            {
                AudioSource src = gameObject.AddComponent<AudioSource>();
                src.clip = clip;
                src.Play();

                StartCoroutine(DestroyTempAudioSource(src));
            }
        if (loop)
        {
            AudioSource src = gameObject.AddComponent<AudioSource>();
            src.clip = clip;
            clipSourceMap[clip.name] = src;
            src.loop = true;
            src.Play();
        }
    }

    /// <summary>
    /// Stops the looping SFX provided
    /// </summary>
    /// <param name="clip"></param>
    public void StopSFX(AudioClip clip)
    {
        if (clip != null)
        {
            if (clipSourceMap.ContainsKey(clip.name))
            {
                clipSourceMap[clip.name].Stop();
                StartCoroutine(DestroyTempAudioSource(clipSourceMap[clip.name], true));
                clipSourceMap.Remove(clip.name);
            }
        }
    }
    private IEnumerator DestroyTempAudioSource(AudioSource source, bool immediate = false)
    {
        if (!immediate)
        {
            yield return new WaitForSeconds(source.clip.length);
            Destroy(source);
        }
        else
        {
            Destroy(source);
        }

    }

    /// <summary>
    ///  Switches BGM to the inputted music AudioClip and plays it on loop.
    /// </summary>
    /// <param name="music"></param>
    public void ChangeBGM(AudioClip music)
    {
        if (music != null && music.name != musicSource.clip.name)
        {
            musicSource.Stop();
            musicSource.clip = music;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    /// <summary>
    ///  Switches to the default BGM and begins playing. 
    /// </summary>
    public void PlayDefaultBGM()
    {
        ChangeBGM(musicClip);
    }
}
