using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSizeManager : MonoBehaviour
{
    [SerializeField] private CameraBounds cameraBounds;
    [SerializeField] private Transform minBounds;
    [SerializeField] private Transform maxBounds;

    public CameraBounds CameraBounds => cameraBounds;
    public Transform MinBounds => minBounds;
    public Transform MaxBounds => maxBounds;
    
    public void SetBoundsForFishNumber(int fishNumber)
    {
        float baseMinX = -8f;
        float baseMaxX = 8f;

        float maxWorldMagnitude = 42f;
        float maxCameraMagnitude = 35f;

        float worldIncrement = 8f * Mathf.Clamp((fishNumber - 20) / 20f, 0, 5);
        float newMinX = Mathf.Max(baseMinX - worldIncrement, -maxWorldMagnitude);
        float newMaxX = Mathf.Min(baseMaxX + worldIncrement, maxWorldMagnitude);

        SetMinXBound(newMinX);
        SetMaxXBound(newMaxX);

        float cameraMin = 0f;
        float cameraMax = 0f;

        if (fishNumber > 20)
        {
            if (fishNumber <= 40)
            {
                cameraMin = Mathf.Lerp(0, -8, (fishNumber - 20) / 20f);
                cameraMax = Mathf.Lerp(0, 8, (fishNumber - 20) / 20f);
            }
            else if (fishNumber <= 80)
            {
                cameraMin = Mathf.Lerp(-8, -25, (fishNumber - 40) / 40f);
                cameraMax = Mathf.Lerp(8, 25, (fishNumber - 40) / 40f);
            }
            else
            {
                cameraMin = Mathf.Lerp(-25, -35, (fishNumber - 80) / 40f);
                cameraMax = Mathf.Lerp(25, 35, (fishNumber - 80) / 40f);
            }
        }

        SetMinCameraBound(cameraMin);
        SetMaxCameraBound(cameraMax);
    }



    private void SetMinXBound(float newMinX)
    {
        if (minBounds != null)
        {
            Vector3 position = minBounds.position;
            position.x = newMinX;
            minBounds.position = position;
        }
    }

    private void SetMinCameraBound(float newMin)
    {
        if (cameraBounds != null)
        {
            cameraBounds.MinX = newMin;
        }
    }

    private void SetMaxXBound(float newMaxX)
    {
        if (maxBounds != null)
        {
            Vector3 position = maxBounds.position;
            position.x = newMaxX;
            maxBounds.position = position;
        }
    }
    
    private void SetMaxCameraBound(float newMax)
    {
        if (cameraBounds != null)
        {
            cameraBounds.MaxX = newMax;
        }
    }
    
}
