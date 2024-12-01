using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFlip : MonoBehaviour
{
    public List<SpriteRenderer> sprites;
    public SpriteRenderer emotionSpriteRenderer;

    public void FaceDirection(Vector3 direction)
    {
        bool toTheRight = direction.x > 0;
        if (toTheRight)
        {
            FaceRight();
        }
        else
        {
            FaceLeft();
        }
    }

    public void FaceDirection(Vector2 direction)
    {
        FaceDirection(new Vector3(direction.x, direction.y, 0));
    }

    public void FaceRight()
    {
        foreach (var sprite in sprites)
        {
            sprite.flipX = false;
        }

        AdjustEmotionXValue(true);

    }

    public void FaceLeft()
    {
        foreach (var sprite in sprites)
        {
            sprite.flipX = true;
        }

        AdjustEmotionXValue(false);
    }
    
    private void AdjustEmotionXValue(bool right)
    {
        Vector3 currentPosition = emotionSpriteRenderer.gameObject.transform.localPosition;

        if (right)
        {
            currentPosition.x = Mathf.Abs(currentPosition.x);
        }
        else
        {
            currentPosition.x = -Mathf.Abs(currentPosition.x);
        }

        emotionSpriteRenderer.gameObject.transform.localPosition = currentPosition;
    }
}