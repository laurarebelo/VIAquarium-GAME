using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReviveButton : MonoBehaviour
{
    public Grave grave;
    private Button reviveButton;
    private GameObject blackScreenGameObject;
    private Image blackScreenImage;
    private DeadFishManager deadFishManager;

    void Start()
    {
        reviveButton = GetComponent<Button>();
        reviveButton.onClick.AddListener(OnReviveButtonPressed);
        deadFishManager = GameObject.Find("DeadFishManager").GetComponent<DeadFishManager>();
    }

    void OnReviveButtonPressed()
    {
        RestartGameState.Instance.deadFishPlaying = grave.deadFish;
        StartCoroutine(deadFishManager.FadeToBlackAndLoadReviveScene(2f));
    }
}