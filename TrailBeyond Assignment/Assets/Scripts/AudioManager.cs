using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<CustomAudioClip> audioClips;

    public static AudioManager Instance { get; private set; }

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlayAudioClip(SoundsName audioName)
    {
        audioClips.ForEach(clip =>
        {
            if (clip.soundName == audioName)
            {
                audioSource.PlayOneShot(clip.audioClip);
                return;
            }
        });
    }

    public void Stop()
    {
        audioSource.Stop();
    }
}
