using System;
using UnityEngine;
using System.Collections;

public class FishEmotions : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer animationSpriteRenderer;
    public GameObject emotionAnimation;
    
    public enum Emotion
    {
        Lonely,
        Happy,
        Loved,
        Hungry
    }

    void Start()
    {
        animator = emotionAnimation.GetComponent<Animator>();
        animationSpriteRenderer = emotionAnimation.GetComponent<SpriteRenderer>();
        animationSpriteRenderer.enabled = false;
    }

    public void SetEmotion(Emotion emotion)
    {
        animationSpriteRenderer.enabled = true; 
        StartCoroutine(PlayEmotionAndHideAfterDuration(emotion));
    }

    private IEnumerator PlayEmotionAndHideAfterDuration(Emotion emotion)
    {
        switch (emotion)
        {
            case Emotion.Hungry:
                animator.SetTrigger("Hungry");
                break;

            case Emotion.Lonely:
                animator.SetTrigger("Lonely");
                break;

            case Emotion.Happy:
                animator.SetTrigger("Happy");
                break;

            case Emotion.Loved:
                animator.SetTrigger("Loved");
                break;

            default:
                Debug.LogWarning("Emotion not recognized: " + emotion);
                break;
        }

        float animationDuration = 5f;
        yield return new WaitForSeconds(animationDuration);
        animationSpriteRenderer.enabled = false;

    }
}
