using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishState : MonoBehaviour
{
    private FishFoodSeek foodSeek;
    private RandomIdleSwim idleSwim;
    void Start()
    {
        foodSeek = GetComponent<FishFoodSeek>();
        idleSwim = GetComponent<RandomIdleSwim>();
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
}
