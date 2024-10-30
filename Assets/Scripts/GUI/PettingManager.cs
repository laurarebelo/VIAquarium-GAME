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
            Cursor.SetCursor(cursorAnimation.texturesArray[currentFrame], cursorAnimation.offset, CursorMode.Auto);
         }
      }
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
      Debug.Log($"Setting cursor type to: {cursorType}");
      SetActiveCursorAnimation(GetCursorAnimation(cursorType));
   }

   
}

[System.Serializable]
public class CursorAnimation
{
   public PettingManager.CursorType CursorType; 
   public Texture2D[] texturesArray;
   public float frameRate;
   public Vector2 offset;


}

