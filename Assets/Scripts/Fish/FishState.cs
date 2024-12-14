using System.Collections;
using System.Collections.Generic;
using Fish;
using UnityEngine;

public class FishState : MonoBehaviour
{
    private FishFoodSeek foodSeek;
    private FishMovement idleSwim;
    private FishKnockReaction fishKnockReaction;
    private FishEmotions fishEmotions;
    private Pettable fishPettable;
    void Start()
    {
        foodSeek = GetComponent<FishFoodSeek>();
        idleSwim = GetComponent<FishMovement>();
        fishKnockReaction = GetComponent<FishKnockReaction>();
        fishEmotions = GetComponent<FishEmotions>();
        fishPettable = GetComponent<Pettable>();
    }

    public void Die()
    {
        StopIdling();
        StopSeekingFood();
        StopShowingEmotions();
        StopReactingToKnocks();
        StopBeingPettable();
    }

    public void StartShowingEmotions()
    {
        fishEmotions.enabled = true;
    }

    public void StopBeingPettable()
    {
        fishPettable.enabled = false;
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
