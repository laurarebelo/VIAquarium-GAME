using System.Collections;
using UnityEngine;

public class FishEmotions : MonoBehaviour
{
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
}