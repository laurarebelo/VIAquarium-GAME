using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGameState : MonoBehaviour
{
    public static RestartGameState Instance { get; private set; }
    public DeadFishGetObject deadFishPlaying;
    public bool isFirstTime = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}