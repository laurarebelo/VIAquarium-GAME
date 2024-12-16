using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Knocker : MonoBehaviour
{
    private Camera mainCamera;
    public float doubleTapTime = 0.3f;
    private float lastTapTime = 0f;
    public CursorManager cursorManager;
    public FeedingManager feedingManager;
    public event Action<Vector3> OnDoubleTap;
    public GameObject knockFeedback;

    public AudioClip[] knockClips;
    private AudioSource audioSource;

    void Start()
    {
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!feedingManager.feedingModeOn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Time.time - lastTapTime <= doubleTapTime)
                {
                    Vector3 position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    OnDoubleTap?.Invoke(position);
                    cursorManager.Knock();
                    Vector3 feedbackPosition = position + new Vector3(0, 0, 11);
                    Instantiate(knockFeedback, feedbackPosition, Quaternion.identity);
                    PlayKnockClip();
                }

                lastTapTime = Time.time;
            }
        }
    }
    
    private void PlayKnockClip()
    {
        if (audioSource != null)
        {
            audioSource.clip = knockClips[Random.Range(0, knockClips.Length)];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioClip or AudioSource is missing.");
        }
    }
}