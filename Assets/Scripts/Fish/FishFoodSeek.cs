using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public class FishFoodSeek : MonoBehaviour
{
    public float detectionRadius = 1.5f;
    public float swimSpeed = 2f;
    public float rampageTimeout = 0.5f;
    private FlakeBehavior targetFlake;

    private int rampageCount = 0;
    private float rampageTimer = 0f;

    private FishController fishController;
    private FishFlip fishFlip;
    private FishState fishState;
    private FishEmotions fishEmotions;
    private FishAudioPlayer audioPlayer;

    private bool isHungry;
    private int minutesToGetHungry = 100;

    private void Start()
    {
        fishController = GetComponent<FishController>();
        fishFlip = GetComponent<FishFlip>();
        fishState = GetComponent<FishState>();
        fishEmotions = GetComponent<FishEmotions>();
        audioPlayer = GameObject.Find("FishAudioPlayer").GetComponent<FishAudioPlayer>();
    }

    void Update()
    {
        FindFoodFlake();
        MoveTowardsFlake();
        HandleRampage();
    }

    void FindFoodFlake()
    {
        if (fishController.hungerLevel == 100) return;
        Collider2D[] foodFlakes = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        targetFlake = null;

        foreach (var flake in foodFlakes)
        {
            if (flake.CompareTag("FoodFlake"))
            {
                FlakeBehavior foundFlake = flake.GetComponent<FlakeBehavior>();
                if (foundFlake.Claim(fishController.fishId)) targetFlake = foundFlake;
                break;
            }
        }
    }

    void MoveTowardsFlake()
    {
        if (targetFlake)
        {
            fishState.StopIdling();
            Vector3 fishPosition = transform.position;
            Vector3 flakePosition = targetFlake.gameObject.transform.position;

            float originalZ = fishPosition.z;

            Vector2 direction = new Vector2(flakePosition.x - fishPosition.x, 0).normalized;
            fishFlip.FaceDirection(direction);
            transform.position = Vector2.MoveTowards(fishPosition, flakePosition, swimSpeed * Time.deltaTime);

            transform.position = new Vector3(transform.position.x, transform.position.y, originalZ);

            // if the fish reaches the flake
            if (Vector2.Distance(new Vector2(fishPosition.x, fishPosition.y),
                    new Vector2(flakePosition.x, flakePosition.y)) < 0.1f)
            {
                Destroy(targetFlake.gameObject);
                rampageCount++;
                rampageTimer = 0f;
                audioPlayer.PlayEatClip();
            }
        }
    }


    void HandleRampage()
    {
        if (!targetFlake)
        {
            rampageTimer += Time.deltaTime;
        }

        if (rampageTimer >= rampageTimeout && rampageCount > 0)
        {
            fishEmotions.SetEmotion(FishEmotions.Emotion.Happy);
            fishState.StartIdling();
            _ = FishAPI.Instance.UploadFishFeed(fishController.fishId, rampageCount);
            int newHungerLevel = fishController.hungerLevel + rampageCount;
            fishController.SetHungerLevel(newHungerLevel);
            Banner.Instance.ShowThankfulMessage(fishController, Banner.NeedType.Hunger, newHungerLevel);
            rampageCount = 0;
            audioPlayer.PlayBeenFedClip();
        }
    }
}