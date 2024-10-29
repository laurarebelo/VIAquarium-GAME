using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickableTrigger : MonoBehaviour
{
    private Camera eventCamera;
    private GUIManager guiManager;
    private FishController fishController;
    private bool selected = false;
    private FishState fishState;

    void Start()
    {
        eventCamera = Camera.main;
        fishController = GetComponent<FishController>();
        fishState = GetComponent<FishState>();
        guiManager = GameObject.Find("GUIManager").GetComponent<GUIManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = eventCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                OnClick();
            }
        }
    }

    private void OnClick()
    {
        selected = !selected;
        if (selected)
        {
            guiManager.SelectFish(fishController);
            fishState.StopIdling();
        }
        else
        {
            guiManager.DeselectFish();
            fishState.StartIdling();
        }
    }
}