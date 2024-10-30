using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Camera eventCamera;
    private GUIManager guiManager;
    private FishController fishController;
    private FishState fishState;
    private bool isPressed = false;
    private bool isSelected = false;
    private float pressDuration = 0.5f; // Time threshold for press vs. click
    private float pressTimer;

    void Start()
    {
        eventCamera = Camera.main;
        fishController = GetComponent<FishController>();
        fishState = GetComponent<FishState>();
        guiManager = GameObject.Find("GUIManager").GetComponent<GUIManager>();
    }

    void Update()
    {
        if (isPressed)
        {
            pressTimer += Time.deltaTime;
            if (pressTimer >= pressDuration)
            {
                if (!isSelected)
                {
                    isSelected = true;
                    guiManager.SelectFish(fishController);
                    fishState.StopIdling();
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        pressTimer = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;

        if (pressTimer < pressDuration)
        {
            OnClick();
        }
        else
        {
            if (isSelected)
            {
                guiManager.DeselectFish();
                fishState.StartIdling();
                isSelected = false;
            }
        }

        pressTimer = 0f;
    }

    private void OnClick()
    {
        if (isSelected)
        {
            guiManager.DeselectFish();
            fishState.StartIdling();
            isSelected = false;
        }
        else
        {
            guiManager.SelectFish(fishController);
            fishState.StopIdling();
            isSelected = true;
        }
    }
}
