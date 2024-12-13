using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip pettingClip;
    public AudioClip beenFedClip;
    public AudioClip beenPetClip;
    public AudioClip lonelyClip;
    public AudioClip hungryClip;
    public AudioClip[] eatClips;
    public bool shouldPlay = true;

    void Start()
    {
        audioSource = GameObject.Find("FishAudioPlayer").GetComponent<AudioSource>();
    }

    private void PlayClip(AudioClip clip, float volumePercentage = 100f, bool loop = false)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.volume = Mathf.Clamp01(volumePercentage / 100f);
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioClip or AudioSource is missing.");
        }
    }

    public void StartPlayingPettingClip()
    {
        PlayClip(pettingClip, 100f, true);
    }

    public void StopPlayingPettingClip()
    {
        if (audioSource != null && audioSource.isPlaying && audioSource.clip == pettingClip)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }
    }

    public void PlayBeenFedClip()
    {
        PlayClip(beenFedClip, 25f);
    }

    public void PlayBeenPetClip()
    {
        PlayClip(beenPetClip, 25f);
    }

    public void PlayLonelyClip()
    {
        PlayClip(lonelyClip);
    }

    public void PlayHungryClip()
    {
        PlayClip(hungryClip, 25f);
    }

    public void PlayEatClip()
    {
        if (eatClips != null && eatClips.Length > 0)
        {
            AudioClip randomEatClip = eatClips[Random.Range(0, eatClips.Length)];
            PlayClip(randomEatClip, 25f);
        }
        else
        {
            Debug.LogWarning("No eat clips available to play.");
        }
    }
}