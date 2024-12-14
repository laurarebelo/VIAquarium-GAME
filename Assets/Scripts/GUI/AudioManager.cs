using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Background Music")]
    public AudioSource bgmAudioSource;
    private float bgmOriginalVolume;
    private bool isMusicMuted = false;

    [Header("Sound Effects")]
    public FishAudioPlayer fishAudioPlayer;
    public FlappyFishAudioPlayer flappyFishAudioPlayer;
    public AudioSource[] soundEffectsAudioSources; 
    private bool areSoundsMuted = false;

    void Start()
    {
        if (bgmAudioSource != null)
        {
            bgmOriginalVolume = bgmAudioSource.volume;
        }
        ApplyMutingState();
    }

    public void ToggleMusic()
    {
        AudioState.Instance.isMusicMuted = !AudioState.Instance.isMusicMuted;
        ApplyMutingState();
    }

    public void ToggleSounds()
    {
        AudioState.Instance.areSoundsMuted = !AudioState.Instance.areSoundsMuted;
        ApplyMutingState();
    }

    private void ApplyMutingState()
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.volume = AudioState.Instance.isMusicMuted ? 0 : bgmOriginalVolume;
        }

        if (fishAudioPlayer != null)
        {
            fishAudioPlayer.enabled = !AudioState.Instance.areSoundsMuted;
        }

        if (flappyFishAudioPlayer != null)
        {
            flappyFishAudioPlayer.enabled = !AudioState.Instance.areSoundsMuted;
        }

        foreach (var audioSource in soundEffectsAudioSources)
        {
            if (audioSource != null)
            {
                audioSource.enabled = !AudioState.Instance.areSoundsMuted;
            }
        }
    }
}
