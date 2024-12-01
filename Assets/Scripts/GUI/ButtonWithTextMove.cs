using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonWithTextMove : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform buttonRectTransform;
    public RectTransform textRectTransform;
    public Vector3 moveOffset = new Vector3(0, -6, 0);

    private Vector3 originalTextPosition;
    private Button button;
    private bool lastInteractableState;


    void Start()
    {
        button = GetComponent<Button>();
        if (textRectTransform != null)
        {
            originalTextPosition = textRectTransform.localPosition;
        }

        lastInteractableState = button.interactable;
        UpdateTextPositionBasedOnButtonState();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable && textRectTransform != null)
        {
            textRectTransform.localPosition = originalTextPosition + moveOffset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button.interactable && textRectTransform != null)
        {
            textRectTransform.localPosition = originalTextPosition;
        }
    }

    void Update()
    {
        if (button.interactable != lastInteractableState)
        {
            lastInteractableState = button.interactable;
            UpdateTextPositionBasedOnButtonState();
        }    }

    private void UpdateTextPositionBasedOnButtonState()
    {
        if (textRectTransform != null)
        {
            if (!button.interactable)
            {
                textRectTransform.localPosition = originalTextPosition + moveOffset;
            }
            else
            {
                textRectTransform.localPosition = originalTextPosition;
            }
        }
    }
}