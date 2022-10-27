using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
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
    AudioSource audioSource;
    public Vector2 pitchRandom = Vector2.one;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayStep()
    {
        audioSource.pitch = Random.Range(pitchRandom.x, pitchRandom.y);
        audioSource.PlayOneShot(step);
    }
}
