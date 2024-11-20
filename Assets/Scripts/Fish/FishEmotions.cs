using System;
using UnityEngine;
using System.Collections;

public class FishEmotions : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer animationSpriteRenderer;
    public GameObject emotionAnimation;

    void Start()
    {
        animator = emotionAnimation.GetComponent<Animator>();
        animationSpriteRenderer = emotionAnimation.GetComponent<SpriteRenderer>();
        animationSpriteRenderer.enabled = false;
    }

    public void SetEmotion(string emotion)
    {
        animationSpriteRenderer.enabled = true; 
        StartCoroutine(PlayEmotionAndHideAfterDuration(emotion));
    }

    private IEnumerator PlayEmotionAndHideAfterDuration(string emotion)
    {
        switch (emotion)
        {
            case "Hungry":
                animator.SetTrigger("Hungry");
                break;

            case "Lonely":
                animator.SetTrigger("Lonely");
                break;

            case "Happy":
                animator.SetTrigger("Happy");
                break;

            case "Loved":
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
