using System.Collections;
using System.Collections.Generic;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RespectButton : MonoBehaviour
{
    public Grave grave;
    private FishAPI fishApi;
    private int respectCount;
    private TMP_Text respectText;
    private Button respectButton;
    
    private float lastRespectTime = 0f;
    private int respectsSentInInterval = 0;
    private bool isWaiting = false;

    void Start()
    {
        respectButton = GetComponent<Button>();
        respectButton.onClick.AddListener(OnRespectButtonPressed);
        fishApi = GameObject.Find("FishApi").GetComponent<FishAPI>();
        respectText = grave.respectCountText;
        int.TryParse(respectText.text, out respectCount);
    }
    
    private void OnRespectButtonPressed()
    {
        respectCount++;
        respectText.text = respectCount.ToString();
        lastRespectTime = Time.time;
        respectsSentInInterval++;

        if (!isWaiting)
        {
            StartCoroutine(WaitForInactivity());
        }
    }
    
    private IEnumerator WaitForInactivity()
    {
        isWaiting = true;
        
        while (Time.time - lastRespectTime < 1f)
        {
            yield return null;
        }
        SendRespectData();
        isWaiting = false;
    }
    
    private async void SendRespectData()
    {
        await fishApi.RespectDeadFish(grave.deadFishId, respectsSentInInterval);
        FishStore.Instance.IncrementDeadFishRespectLevel(grave.deadFishId, respectsSentInInterval);
        respectsSentInInterval = 0;
    }
    
    private void OnDestroy()
    {
        if (respectsSentInInterval > 0)
        {
            SendRespectData();
        }
    }
}
