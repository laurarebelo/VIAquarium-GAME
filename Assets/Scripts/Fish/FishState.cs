using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishState : MonoBehaviour
{
    private FishFoodSeek foodSeek;
    private RandomIdleSwim idleSwim;
    private FishDeath deathSequence;

    void Start()
    {
        foodSeek = GetComponent<FishFoodSeek>();
        idleSwim = GetComponent<RandomIdleSwim>();
        deathSequence = GetComponent<FishDeath>();
        idleSwim.enabled = true;
    }

    public void StartEating()
    {
        StopIdling();
    }
    
    public void StopEating()
    {
        StartIdling();
    }

    public void StopIdling()
    {
        idleSwim.enabled = false;
    }

    public void StartIdling()
    {
        idleSwim.enabled = true;
    }

    public void StartDeathSequence()
    {
        StopIdling();
        if (deathSequence != null)
        {
            deathSequence.TriggerDeath();
        }
    }
}