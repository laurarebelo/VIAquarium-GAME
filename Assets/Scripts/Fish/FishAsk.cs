using System;
using UnityEngine;
using System.Collections;

public class FishAsk : MonoBehaviour
{
    private FishController fishController;
    private FishEmotions fishEmotions;

    private float checkInterval = 30f;
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
        if (fishController.hungerLevel < 10)
        {
           fishEmotions.SetEmotion("Hungry");
        }
        else if (fishController.socialLevel < 10)
        {
            fishEmotions.SetEmotion("Lonely");
        }
    }
}