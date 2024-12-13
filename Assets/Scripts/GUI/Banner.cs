using System;
using System.Collections;
using System.Collections.Generic;
using Fish;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Banner : MonoBehaviour
{
    private static Banner instance;
    private RectTransform rectTransform;
    
    public Image fishOutline;
    public Image fishColor;
    public TMP_Text fishName;
    public TMP_Text message;
    
    private float onScreenY = -173f;
    private float offScreenY = -270f;
    [SerializeField] private float moveSpeed = 5f; 
    private Coroutine currentCoroutine;

    private bool isMessageShowing;
    private FishAudioPlayer audioPlayer;

    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        audioPlayer = GameObject.Find("FishAudioPlayer").GetComponent<FishAudioPlayer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Appear();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Disappear();
        }
    }

    private void SetFishOnBanner(FishController fishController)
    {
        fishName.text = fishController.fishName;
        fishOutline.sprite = fishController.fishSprites.outlineSprite;
        fishColor.sprite = fishController.fishColorSpriteRenderer.sprite;
    }

    public void ShowNeedyMessage(FishController fishController, NeedType needType)
    {
        if (isMessageShowing) return;
        isMessageShowing = true;
        SetFishOnBanner(fishController);
        string messageString = "";
        switch (needType)
        {
            case NeedType.Hunger:
                messageString = FishMessages.GetRandomHungerComplaint();
                audioPlayer.PlayHungryClip();
                break;
                case NeedType.Social:
                messageString = FishMessages.GetRandomSocialComplaint();
                audioPlayer.PlayLonelyClip();
                break;
        }
        message.text = messageString;
        AppearAndReappear();
    }

    public void ShowThankfulMessage(FishController fishController, NeedType needType, int points)
    {
        if (isMessageShowing) return;
        isMessageShowing = true;
        SetFishOnBanner(fishController);
        string messageString = "";
        switch (needType)
        {
            case NeedType.Hunger:
                messageString = FishMessages.GetFeedingResponse(points);
                break;
            case NeedType.Social:
                messageString = FishMessages.GetPettingResponse(points);
                break;
        }
        message.text = messageString;
        AppearAndReappear();
    }

    
    public static Banner Instance => instance;
    
    public void Appear()
    {
        StartMove(onScreenY);
    }

    public void Disappear()
    {
        StartMove(offScreenY);
        isMessageShowing = false;
    }
    
    public void AppearAndReappear()
    {
        Appear();
        StartCoroutine(DisappearAfterDelay(5f));
    }

    private IEnumerator DisappearAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Disappear();
    }
    
    private void StartMove(float targetY)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(SmoothMove(targetY));
    }
    
    private IEnumerator SmoothMove(float targetY)
    {
        Vector2 currentPos = rectTransform.anchoredPosition;
        Vector2 targetPos = new Vector2(currentPos.x, targetY);

        while (Vector2.Distance(rectTransform.anchoredPosition, targetPos) > 0.01f)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPos, Time.deltaTime * moveSpeed);
            yield return null;
        }

        rectTransform.anchoredPosition = targetPos;
        currentCoroutine = null;
    }

    public enum NeedType
    {
        Hunger,
        Social
    }
    
}
