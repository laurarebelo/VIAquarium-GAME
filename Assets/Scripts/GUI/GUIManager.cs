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
    public Button ContinueButton;
    public Button DeleteButton;
    public Button FeedButton;

    public GameObject AddFishCanvas;

    public FishManager fishManager;
    public FeedingManager feedingManager;

    [CanBeNull] public FishController selectedFish;

    void Start()
    {
        AddButton.onClick.AddListener(() => ToggleScreen(AddFishCanvas));
        ContinueButton.onClick.AddListener(GoToFishPainting);
        DeleteButton.onClick.AddListener(DeleteFish);
        FeedButton.onClick.AddListener(ToggleFeedingMode);
    }

    void DeleteFish()
    {
        if (selectedFish)
        {
            fishManager.DeleteFish(selectedFish);
            DeselectFish();
        }
    }

    void ToggleFeedingMode()
    {
        feedingManager.ToggleFeedingMode();
    }

    void ToggleScreen(GameObject screen, bool? active = null)
    {
        if (active == null)
        {
            active = !screen.activeSelf;
        }

        screen.SetActive(active.Value);
    }

    void GoToFishPainting()
    {
        SceneManager.LoadScene("DrawingCanvas");
    }

    public void SelectFish(FishController fish)
    {
        if (selectedFish)
        {
            selectedFish.Deselect();
        }
        
        selectedFish = fish;
        fish.Select();
        fish.GetComponent<FishState>().StopIdling();
    }

    public void DeselectFish()
    {
        if (selectedFish)
        {
            selectedFish.Deselect();
            selectedFish.GetComponent<FishState>().StartIdling();
            selectedFish = null;
        }
    }
}
