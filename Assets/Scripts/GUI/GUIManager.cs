using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public Button AddButton;
    public Button DeleteButton;
    public Button FeedButton;
    public Button ContinueButton;

    public GameObject AddFishCanvas;
    public FishTemplateProvider fishTemplateProvider;

    public FishManager fishManager;
    public FeedingManager feedingManager;

    [CanBeNull] public FishController selectedFish;

    void Start()
    {
        fishTemplateProvider.OnTemplateSelectionChanged.AddListener(UpdateContinueButtonState);
        AddButton.onClick.AddListener(() => ToggleAddFishCanvas());
        ContinueButton.onClick.AddListener(GoToFishPainting);
        ContinueButton.interactable = false;
        DeleteButton.onClick.AddListener(DeleteFish);
        FeedButton.onClick.AddListener(ToggleFeedingMode);
    }

    void DeleteFish()
    {
        if (selectedFish)
        {
            fishManager.DeleteFish(selectedFish);
        }
    }

    void ToggleFeedingMode()
    {
        feedingManager.ToggleFeedingMode();
    }

    public void SelectFish(FishController fish)
    {
        // Deselect the currently selected fish, if any
        if (selectedFish != null)
        {
            DeselectFish();
        }

        // Set the new selected fish and stop idling
        selectedFish = fish;
        selectedFish.GetComponent<FishState>().StopIdling();
        selectedFish.Select();
    }

    public void DeselectFish()
    {
        if (selectedFish != null)
        {
            selectedFish.Deselect();
            selectedFish.GetComponent<FishState>().StartIdling();
        }

        selectedFish = null;
    }

    void ToggleAddFishCanvas(bool? active = null)
    {
        if (active == null)
        {
            active = !AddFishCanvas.activeSelf;
        }
        AddFishCanvas.SetActive(active.Value);
        if (!active.Value)
        {
            fishTemplateProvider.DeselectTemplate();
        }
    }

    void GoToFishPainting()
    {
        SceneManager.LoadScene("DrawingCanvas");
    }

    void UpdateContinueButtonState(bool isTemplateSelected)
    {
        ContinueButton.interactable = isTemplateSelected;
    }
}