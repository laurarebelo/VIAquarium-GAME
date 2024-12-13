using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedingManager : MonoBehaviour
{
    private AudioSource audioSource;
    public GameObject flakeParticleSystemPrefab;
    public bool feedingModeOn;
    public Sprite spriteFeedingOff;
    public Sprite spriteFeedingOn;
    public Image feedingButtonImage;
    public CursorManager cursorManager;

    void Start()
    {
        audioSource = GameObject.Find("FishAudioPlayer").GetComponent<AudioSource>();
    }

    public void ToggleFeedingMode()
    {
        feedingModeOn = !feedingModeOn;
        feedingButtonImage.sprite = feedingModeOn ? spriteFeedingOn : spriteFeedingOff;
        if (feedingModeOn)
        {
            cursorManager.EnterFeedingMode();
        }
        else
        {
            cursorManager.DefaultHand();
        }
    }

    void Update()
    {
        if (feedingModeOn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                cursorManager.Feed();
                Vector3 tapPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                tapPosition.z = 0;
                InstantiateFlakeParticleSystem(tapPosition);
            }
        }
    }

    void InstantiateFlakeParticleSystem(Vector3 position)
    {
        GameObject flakeParticleSystem = Instantiate(flakeParticleSystemPrefab, position, Quaternion.identity);
        audioSource.Play();
    }
}