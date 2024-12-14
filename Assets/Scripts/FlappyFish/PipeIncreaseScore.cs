using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeIncreaseScore : MonoBehaviour
{
    public SpriteRenderer circle;
    public Sprite loveSprite;
    public Sprite[] foodSprites;
    private Gradient startColor;
    private FlappyFishAudioPlayer audioPlayer;

    void Start()
    {
        audioPlayer = GameObject.Find("FlappyFishAudioPlayer").GetComponent<FlappyFishAudioPlayer>();
        startColor = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[3];
        colorKeys[0] = new GradientColorKey(Color.green, 0.0f);
        colorKeys[1] = new GradientColorKey(Color.yellow, 0.5f);
        colorKeys[2] = new GradientColorKey(Color.red, 1.0f);
        startColor.colorKeys = colorKeys;
        if (RestartGameState.Instance.deadFishPlaying.causeOfDeath.ToLower() == "loneliness")
        {
            circle.sprite = loveSprite;
            circle.color = Color.white;
        }
        else
        {
            circle.sprite = foodSprites[Random.Range(0, foodSprites.Length)];
            float gradientValue = Random.Range(0f, 1f);
            circle.color = startColor.Evaluate(gradientValue);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Score.instance.UpdateScore();
            circle.color = new Color(0, 0, 0, 0);
            if (RestartGameState.Instance.deadFishPlaying.causeOfDeath.ToLower() == "loneliness")
            {
                audioPlayer.PlayHeartClip();
            }
            else
            {
                audioPlayer.PlayEatClip();
            }
        }
    }
}
