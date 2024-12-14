using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyFishAudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip heartPickupClip;
    public AudioClip deathClip;
    public AudioClip reviveClip;
    public AudioClip[] eatClips;
    
    void Start()
    {
        audioSource = GameObject.Find("FlappyFishAudioPlayer").GetComponent<AudioSource>();
    }

    private void PlayClip(AudioClip clip, float volumePercentage = 100f, bool loop = false)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.Stop();
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

    public void PlayHeartClip()
    {
        PlayClip(heartPickupClip, 25f);
    }

    public void PlayDeathClip()
    {
        PlayClip(deathClip, 25f);
    }

    public void PlayReviveClip()
    {
        PlayClip(reviveClip, 25f);
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
