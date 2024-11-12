using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Model;

public class CursorObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private FishAPI fishApi;
    private FishController fishController;
    private PettingManager pettingManager;
    private int holdCounter = 0;
    private Coroutine holdCounterCoroutine;
    private FishEmotions fishEmotions;

    private void Start()
    {
        fishApi = GameObject.Find("FishApi").GetComponent<FishAPI>();
        fishController = GetComponent<FishController>();
        pettingManager = FindObjectOfType<PettingManager>();
        
        if (pettingManager == null)
        {
            Debug.LogError("PettingManager not found in the scene.");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (pettingManager != null)
        {
            pettingManager.SetActiveCursorType(PettingManager.CursorType.Petting);
            holdCounterCoroutine = StartCoroutine(CountHoldTime());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (pettingManager != null)
        {
            pettingManager.SetActiveCursorType(PettingManager.CursorType.DefaultHand);

            if (holdCounterCoroutine != null && holdCounter >= 5)
            {
                StopCoroutine(holdCounterCoroutine);
                //fishEmotions.UpdateHappyEmotion();
                _ = fishApi.UploadFishPet(fishController.fishId, holdCounter);
                holdCounter = 0;
            }
        }
    }


    private IEnumerator CountHoldTime()
    {
        holdCounter = 0;
        while (true)
        {
            yield return new WaitForSeconds(1);
            holdCounter+=5;
        }
    }
}