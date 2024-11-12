using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFlip : MonoBehaviour
{
    public List<SpriteRenderer> sprites;

    public void FaceDirection(Vector3 direction)
    {
        bool toTheRight = direction.x > 0;
        bool flipFishVertically = direction.y > 0;
        if (toTheRight)
        {
            FaceRight();
        }
        else
        {
            FaceLeft();
        }
        
        if (flipFishVertically)
        {
            FlipVertically();
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
    }

    public void FaceLeft()
    {
        foreach (var sprite in sprites)
        {
            sprite.flipX = true;
        }
    }
    
    public void FlipVertically()
    {
        foreach (var sprite in sprites)
        {
            sprite.flipY = true;
        }
    }
    
}