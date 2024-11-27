using System;
using UnityEngine;
using System.Collections;

public class FishAsk : MonoBehaviour
{
    private FishController fishController;
    private FishEmotions fishEmotions;

    public float checkInterval = 30f;
    void Start()
    {   
        fishController = gameObject.GetComponent<FishController>();
        fishEmotions = gameObject.GetComponent<FishEmotions>();
        StartCoroutine(CheckFishStatusCoroutine());
    }

    IEnumerator CheckFishStatusCoroutine()
    {
        while (true)
        {
            CheckFishStatus();
            yield return new WaitForSeconds(checkInterval);
        }
    }

    void CheckFishStatus()
    {
        if (fishController.hungerLevel < 30)
        {
           fishEmotions.SetEmotion(FishEmotions.Emotion.Hungry);
        }
        else if (fishController.socialLevel < 30)
        {
            fishEmotions.SetEmotion(FishEmotions.Emotion.Lonely);
        }
    }
}