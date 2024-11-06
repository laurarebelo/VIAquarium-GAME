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

            if (holdCounterCoroutine != null)
            {
                StopCoroutine(holdCounterCoroutine);
                _ = fishApi.UploadFishNeed(fishController.fishId,"hunger", holdCounter);
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
            holdCounter++;
            Debug.Log("Hold time: " + holdCounter + " seconds."); //1 sec = 1point? might change
        }
    }
}