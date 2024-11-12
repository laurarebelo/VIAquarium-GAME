using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
public class ClickableTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private GUIManager guiManager;
    private FishController fishController;
    private bool isPressed = false;
    private float pressDuration = 0.2f; // Time threshold for press vs. click
    private float pressTimer;

    void Start()
    {
        fishController = GetComponent<FishController>();
        guiManager = GameObject.Find("GUIManager").GetComponent<GUIManager>();
    }

    void Update()
    {
        if (isPressed)
        {
            pressTimer += Time.deltaTime;
            if (pressTimer >= pressDuration && guiManager.selectedFish != fishController)
            {
                guiManager.SelectFish(fishController);
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
            if (guiManager.selectedFish == fishController)
            {
                guiManager.DeselectFish();
            }
            else
            {
                guiManager.SelectFish(fishController);
            }
        }
        
        pressTimer = 0f;
    }
}

