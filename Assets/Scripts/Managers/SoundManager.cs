using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Serializable]
    public class ClipsDict : UnitySerializedDictionary<string, AudioClip> { }

    public ClipsDict musicDict;
    public ClipsDict soundDict;

    public static SoundManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"??? Multiple {instance} singletons");
            return;
        }
        instance = this;
    }

    public AudioClip step;
    public AudioSource audioSourceMusic;
    public AudioSource audioSourceSounds;
    public AudioSource audioSourceSteps;
    public AudioSource audioSourceLaptop;
    public AudioSource audioSourceText;
    public Vector2 pitchRandom = Vector2.one;
    public Vector2 pitchText = Vector2.one;

    public void PlayStep()
    {
        audioSourceSteps.pitch = UnityEngine.Random.Range(pitchRandom.x, pitchRandom.y);
        audioSourceSteps.PlayOneShot(soundDict["footstep"]);
    }

    public void PlayTextSound()
    {
        audioSourceText.pitch = UnityEngine.Random.Range(pitchText.x, pitchText.y);
        audioSourceText.PlayOneShot(soundDict["text_sound"]);
    }

    public void PlaySoundOneShot(string clipName)
    {
        if(!soundDict.ContainsKey(clipName))
        {
            Debug.LogWarning("Sounds dict doesn't contain " + clipName);
            return;
        }
        audioSourceSounds.PlayOneShot(soundDict[clipName]);
    }

    public void PlaySound(string clipName, bool loop = false)
    {
        if (!soundDict.ContainsKey(clipName))
        {
            Debug.LogWarning("Sounds dict doesn't contain " + clipName);
            return;
        }
        audioSourceSounds.loop = loop;

        audioSourceSounds.clip = soundDict[clipName];
        audioSourceSounds.Play();
    }

    public void PlayLaptopAmbience(string clipName)
    {
        if (!musicDict.ContainsKey(clipName))
        {
            Debug.LogWarning("Sounds dict doesn't contain " + clipName);
            return;
        }
        audioSourceLaptop.clip = musicDict[clipName];
        audioSourceLaptop.Play();
    }

    public void StopLaptopAmbience()
    {
        audioSourceLaptop.Stop();
    }

    public void PlayMusic(string clipName)
    {

        if (!musicDict.ContainsKey(clipName))
        {
            Debug.LogWarning("Music dict doesn't contain " + clipName);
            return;
        }
        audioSourceMusic.clip = musicDict[clipName];
        audioSourceMusic.Play();
    }
}
