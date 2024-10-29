using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedingManager : MonoBehaviour
{
    public GameObject flakeParticleSystemPrefab;
    private bool feedingModeOn;

    public void ToggleFeedingMode()
    {
        feedingModeOn = !feedingModeOn;
    }

    void Update()
    {
        if (feedingModeOn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 tapPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                tapPosition.z = 0;
                InstantiateFlakeParticleSystem(tapPosition);
            }
        }
    }
    
    void InstantiateFlakeParticleSystem(Vector3 position)
    {
        GameObject flakeParticleSystem = Instantiate(flakeParticleSystemPrefab, position, Quaternion.identity);
    }
}