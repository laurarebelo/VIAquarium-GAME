using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Score : MonoBehaviour
{
    public static Score instance;
    public TMP_Text scoreNumberTMP;

    public int score;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        scoreNumberTMP.text = score.ToString();
    }

    public void UpdateScore()
    {
        score++;
        scoreNumberTMP.text = score.ToString();
    }
}
