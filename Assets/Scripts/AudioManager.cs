using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
 
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private List<AudioSource> audioSources = new List<AudioSource>();
 
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("AudioManager is null");
            }
            return instance;
        }
    }
 
    private void Awake()
    { 
        instance = this;
    }
 
    public void PlaySound(AudioClip clipToPlay, bool randomPitch = false, float volume = 0.4f)
    {
        audioSources.Add(this.AddComponent<AudioSource>());
        var audio = audioSources[audioSources.Count - 1];
        audio.volume = volume;
        audio.clip = clipToPlay;
        if(randomPitch)
        {
            audio.pitch = Random.Range(0.8f, 1.2f);
        }
        audio.Play();
        Destroy(audio, clipToPlay.length);
    }
 
    public void PlaySoundLooping(AudioClip clipToPlay)
    {
        audioSources.Add(this.AddComponent<AudioSource>());
        var audio = audioSources[audioSources.Count - 1];
        audio.clip = clipToPlay;
        audio.loop = true;
        audio.Play();
    }
 
    public void StopSoundLooping()
    {
        foreach (var audio in this.GetComponents<AudioSource>())
        {
            if(audio.loop == true)
            {
                audio.loop = false;
                audio.Stop();
                Destroy(audio);
            }
        }
    }
}