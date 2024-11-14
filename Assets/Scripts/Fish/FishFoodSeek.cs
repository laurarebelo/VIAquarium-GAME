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

    private FishAPI fishApi;
    private FishController fishController;
    private FishFlip fishFlip;
    private FishState fishState;

    private int potentialHunger;
    private bool isHungry;
    private int minutesToGetHungry = 100;

    private void Start()
    {
        fishApi = GameObject.Find("FishApi").GetComponent<FishAPI>();
        fishController = GetComponent<FishController>();
        fishFlip = GetComponent<FishFlip>();
        fishState = GetComponent<FishState>();
        potentialHunger = fishController.hungerLevel;
        StartCoroutine(DecreaseHungerOverTime());
        CheckHunger();
    }

    void Update()
    {
        CheckHunger();
        FindFoodFlake();
        MoveTowardsFlake();
        HandleRampage();
    }

    void CheckHunger()
    {
        isHungry = potentialHunger < 100;
    }

    void FindFoodFlake()
    {
        if (!isHungry) return;
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
                potentialHunger++;
                rampageCount++;
                rampageTimer = 0f;
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
            fishState.StartIdling();
            _ = fishApi.UploadFishFeed(fishController.fishId, rampageCount);
            rampageCount = 0;
        }
    }

    private IEnumerator DecreaseHungerOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(minutesToGetHungry * 60f);
            potentialHunger = Mathf.Max(0, potentialHunger - 1);
            CheckHunger();
        }
    }
}