using UnityEngine;
using System.Collections;

public class FishEmotions : MonoBehaviour
{
    private Animator animator;
    public GameObject emotionAnimation;
    public int playForSeconds;
    void Start()
    {
        animator = emotionAnimation.GetComponent<Animator>();
    }

    public void SetEmotion(string emotion)
    {
        StartCoroutine(PlayEmotionAndReturnToIdle(emotion));
    }

    private IEnumerator PlayEmotionAndReturnToIdle(string emotion)
    {
        animator.SetTrigger("Idle"); 

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

        yield return new WaitForSeconds(playForSeconds);

        animator.SetTrigger("Idle");
    }
}