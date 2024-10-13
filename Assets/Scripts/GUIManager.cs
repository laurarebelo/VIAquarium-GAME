using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Model;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public Button AddButton;
    public Button SubmitButton;
    public Button DeleteButton;

    public GameObject AddFishCanvas;

    public FishManager fishManager;

    [CanBeNull] public FishController selectedFish;

    // Start is called before the first frame update
    void Start()
    {
        AddButton.onClick.AddListener(() => ToggleScreen(AddFishCanvas));
        SubmitButton.onClick.AddListener(() => ToggleScreen(AddFishCanvas, false));
        DeleteButton.onClick.AddListener(DeleteFish);
    }

    void DeleteFish()
    {
        if (selectedFish)
        {
            fishManager.DeleteFish(selectedFish);
        }
    }

    void ToggleScreen(GameObject screen, bool? active = null)
    {
        if (active == null)
        {
            active = !screen.activeSelf;
        }

        screen.SetActive(active.Value);
    }

    public void SelectFish(FishController fish)
    {
        if (selectedFish)
        {
            selectedFish.Deselect();
        }

        selectedFish = fish;
        fish.Select();
    }

    public void DeselectFish()
    {
        if (selectedFish)
        {
            selectedFish.Deselect();
        }

        selectedFish = null;
    }
}