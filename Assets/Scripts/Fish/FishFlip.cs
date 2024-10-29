using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFlip : MonoBehaviour
{
    public List<SpriteRenderer> sprites;

    public void FaceDirection(Vector3 direction)
    {
        if (direction == Vector3.left)
        {
            FaceLeft();
        }

        if (direction == Vector3.right)
        {
            FaceRight();
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
}
