using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PettingManager : MonoBehaviour
{
   [SerializeField] private List<CursorAnimation> CursorAnimationList;
   private CursorAnimation cursorAnimation;
   private int currentFrame;
   private float frameTimer;
   private int frameCount;
   
   //can be expanded in case we want other cursor types/animations in the future
   public enum CursorType
   {
      DefaultHand,
      Petting
   }

   private void Awake()
   {
      TurnSpritesToTextures();
   }
   
   private void Start()
   {
      SetActiveCursorType(CursorType.DefaultHand);   
   }
   
   private void Update()
   {
      if (frameCount > 0)
      {
         frameTimer += Time.deltaTime;
         if (frameTimer >= cursorAnimation.frameRate)
         {
            frameTimer -= cursorAnimation.frameRate;
            currentFrame = (currentFrame + 1) % frameCount;
            Texture2D currentFrameTexture = cursorAnimation.texturesArray[currentFrame];
            Cursor.SetCursor(currentFrameTexture, cursorAnimation.offset, CursorMode.Auto);
         }
      }
   }

   private void OnDestroy()
   {
      Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
   }

   private void SetActiveCursorAnimation(CursorAnimation cursorAnimation)
   {
      this.cursorAnimation = cursorAnimation;
      currentFrame = 0;

      if (cursorAnimation.frameRate > 0 && cursorAnimation.texturesArray.Length > 0)
      {
         frameTimer = cursorAnimation.frameRate;
         frameCount = cursorAnimation.texturesArray.Length;
      }
      else
      {
         Debug.LogWarning("CursorAnimation has no textures or invalid frame rate.");
      }
   }

   private CursorAnimation GetCursorAnimation(CursorType cursorType)
   {
      foreach (CursorAnimation cursorAnimation in CursorAnimationList)
      {
         if (cursorAnimation.CursorType == cursorType) return cursorAnimation;
      }
      return null;
   }

   public void SetActiveCursorType(CursorType cursorType)
   {
      SetActiveCursorAnimation(GetCursorAnimation(cursorType));
   }

   private void TurnSpritesToTextures()
   {
      foreach (CursorAnimation curAnim in CursorAnimationList)
      {
         curAnim.texturesArray = new Texture2D[curAnim.spritesArray.Length];
         for (int i = 0; i < curAnim.spritesArray.Length; i++)
         {
            curAnim.texturesArray[i] = SpriteToTexture(curAnim.spritesArray[i]);
         }
      }
   }
   
   private Texture2D SpriteToTexture(Sprite sprite)
   {
      Texture2D spriteTexture = sprite.texture;
      Rect spriteRect = sprite.textureRect;
      Texture2D texture = new Texture2D((int)spriteRect.width, (int)spriteRect.height, TextureFormat.RGBA32, false);
      texture.SetPixels(spriteTexture.GetPixels(
         (int)spriteRect.x, 
         (int)spriteRect.y, 
         (int)spriteRect.width, 
         (int)spriteRect.height));
      texture.Apply();
      return texture;
   }
}

[System.Serializable]
public class CursorAnimation
{
   public PettingManager.CursorType CursorType;
   public Sprite[] spritesArray;
   public Texture2D[] texturesArray;
   public float frameRate;
   public Vector2 offset;
}

