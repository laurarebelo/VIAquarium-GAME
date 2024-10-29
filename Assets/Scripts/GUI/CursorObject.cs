using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorObject : MonoBehaviour
{
   [SerializeField] private PettingManager.CursorType cursorType;

   public void OnMouseDown()
   {
      Debug.Log("Mouse pressed");

      PettingManager.Instance.SetActiveCursorType(cursorType);
   }

   public void OnMouseUp()
   {
      Debug.Log("Mouse exit");
      PettingManager.Instance.SetActiveCursorType(PettingManager.CursorType.DefaultHand);
   }
}