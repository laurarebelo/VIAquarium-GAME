using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    public Image backgroundImage;
    public Sprite backgroundDawn;
    public Sprite backgroundDay;
    public Sprite backgroundDusk;
    public Sprite backgroundNight;
    
    private static readonly TimeSpan dawnStart = new TimeSpan(5, 0, 0);  // 5 AM
    private static readonly TimeSpan dayStart = new TimeSpan(7, 0, 0);   // 7 AM
    private static readonly TimeSpan duskStart = new TimeSpan(18, 0, 0); // 6 PM
    private static readonly TimeSpan nightStart = new TimeSpan(20, 0, 0); // 8 PM
    
    private void Start()
    {
        InvokeRepeating(nameof(UpdateBackground), 0f, 3600f);
        UpdateBackground();
    }
    
    private void UpdateBackground()
    {
        TimeSpan currentTime = DateTime.Now.TimeOfDay;

        if (currentTime >= dawnStart && currentTime < dayStart)
        {
            backgroundImage.sprite = backgroundDawn;
        }
        else if (currentTime >= dayStart && currentTime < duskStart)
        {
            backgroundImage.sprite = backgroundDay;
        }
        else if (currentTime >= duskStart && currentTime < nightStart)
        {
            backgroundImage.sprite = backgroundDusk;
        }
        else
        {
            backgroundImage.sprite = backgroundNight;
        }
    }

    public static Color GetFishColorForTimeOfDay()
    {
        TimeSpan currentTime = DateTime.Now.TimeOfDay;

        if (currentTime >= dawnStart && currentTime < dayStart)
        {
            return new Color(0.9f, 1, 0.8f);
        }
        else if (currentTime >= dayStart && currentTime < duskStart)
        {

            return Color.white;
        }
        else if (currentTime >= duskStart && currentTime < nightStart)
        {
            return new Color(1f, 0.8f, 0.9f);
        }
        else
        {
            return new Color(0.8f, 0.8f, 1f);
        }
    }
    
}