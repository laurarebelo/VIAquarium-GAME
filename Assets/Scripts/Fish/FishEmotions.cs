using System.Collections;
using UnityEngine;

public class FishEmotions : MonoBehaviour
{
    //ok so i thought it was single sprites we were dealing with so the class needs to be completely changed since it needs to handle animation
    
    public SpriteRenderer hungrySprite;
    public SpriteRenderer lonelySprite;
    public SpriteRenderer happySprite;
    public SpriteRenderer heartSprite;
    public int emotionShowTime;
    private FishController fishController;

    void Start()
    {
        fishController = GetComponent<FishController>();
        heartSprite.gameObject.SetActive(false);
        happySprite.gameObject.SetActive(false);
        hungrySprite.gameObject.SetActive(false);
        lonelySprite.gameObject.SetActive(false);
        UpdateLowEmotion();
    }

    public void UpdateLowEmotion()
    {
        int hungerLevel = fishController.hungerLevel;
        int socialLevel = fishController.socialLevel;

        if (hungerLevel > 0 && hungerLevel < 10)
        {
            StartCoroutine(ShowEmotion(hungrySprite));
        }

        if (socialLevel > 0 && socialLevel < 10)
        {
            StartCoroutine(ShowEmotion(lonelySprite));
        }
    }

    public void UpdateHappyEmotion()
    {
        StartCoroutine(ShowEmotion(happySprite));
    }

    public void UpdateHeartEmotion()
    {
        StartCoroutine(ShowEmotion(heartSprite));
    }

    private IEnumerator ShowEmotion(SpriteRenderer sprite)
    {
        sprite.gameObject.SetActive(true);
        yield return new WaitForSeconds(emotionShowTime);
        sprite.gameObject.SetActive(false);
    }
    
    // for animation it might be something more like this:
    // public class FishEmotions : MonoBehaviour
    // {
    //     public Animator thinkingCloudAnimator;
    //
    //     public void SetEmotion(string emotion)
    //     {
    //         switch (emotion)
    //         {
    //             case "Hungry":
    //                 thinkingCloudAnimator.SetTrigger("HungryTrigger");
    //                 break;
    //             case "Lonely":
    //                 thinkingCloudAnimator.SetTrigger("LonelyTrigger");
    //                 break;
    //             case "Happy":
    //                 thinkingCloudAnimator.SetTrigger("HappyTrigger");
    //                 break;
    //             case "Loved":
    //                 thinkingCloudAnimator.SetTrigger("LovedTrigger");
    //                 break;
    //         }
    //     }
    // }
    
    //and we'd call SetEmotion when needed
}