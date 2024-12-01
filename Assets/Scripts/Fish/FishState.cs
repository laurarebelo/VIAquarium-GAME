using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishState : MonoBehaviour
{
    private FishFoodSeek foodSeek;
    private FishMovement idleSwim;
    private FishKnockReaction fishKnockReaction;
    private FishEmotions fishEmotions;
    void Start()
    {
        foodSeek = GetComponent<FishFoodSeek>();
        idleSwim = GetComponent<FishMovement>();
        fishKnockReaction = GetComponent<FishKnockReaction>();
        fishEmotions = GetComponent<FishEmotions>();
    }

    public void Die()
    {
        StopIdling();
        StopSeekingFood();
        StopShowingEmotions();
        StopReactingToKnocks();
    }

    public void StartShowingEmotions()
    {
        fishEmotions.enabled = true;
    }

    public void StopShowingEmotions()
    {
        fishEmotions.enabled = false;
    }

    public void StartReactingToKnocks()
    {
        fishKnockReaction.enabled = true;
    }

    public void StopReactingToKnocks()
    {
        fishKnockReaction.enabled = false;
    }

    public void StartSeekingFood()
    {
        foodSeek.enabled = true;
    }

    public void StopSeekingFood()
    {
        foodSeek.enabled = false;
    }

    public void StopIdling()
    {
        idleSwim.enabled = false;
    }

    public void StartIdling()
    {
        idleSwim.enabled = true;
    }
}
