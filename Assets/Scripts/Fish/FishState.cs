using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishState : MonoBehaviour
{
    private FishFoodSeek foodSeek;
    private FishMovement idleSwim;
    void Start()
    {
        foodSeek = GetComponent<FishFoodSeek>();
        idleSwim = GetComponent<FishMovement>();
        idleSwim.enabled = true;
        foodSeek.enabled = true;
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
