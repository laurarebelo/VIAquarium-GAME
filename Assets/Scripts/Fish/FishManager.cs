using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model;
using TMPro;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public FishAPI fishApi;
    private List<FishGetObject> fishList;
    public GameObject fishPrefab;
    private int z;
    private FishTemplateProvider fishTemplateProvider;
    public GameObject loadingScreen;


    void Start()
    {
        fishTemplateProvider = GameObject.Find("FishTemplateProvider").GetComponent<FishTemplateProvider>();
        _ = InitializeFish();
    }

    async Task InitializeFish()
    {
        if (FishStore.Instance.HasStoredFish())
        {
            InstantiateFishList(FishStore.Instance.GetStoredFish());
        }
        else
        {
            ShowLoadingScreen(true);
            var allFish = await fishApi.GetAllFishAlive();
            FishStore.Instance.StoreFishList(allFish);
            InstantiateFishList(allFish);
            ShowLoadingScreen(false);
        }
    }

    void ShowLoadingScreen(bool show)
    {
        loadingScreen.SetActive(show);
    }

    public void InstantiateFishList(List<FishGetObject> fishList)
    {
        foreach (var fish in fishList)
        {
            InstantiateFish(fish);
        }
    }

public void InstantiateFish(FishGetObject newFish)
    {
        Vector3 position = Utils.GetRandomPosition();
        position.z = z;
        z++;
        GameObject newFishGo = Instantiate(fishPrefab, position, Quaternion.identity);
        FishController fishController = newFishGo.GetComponent<FishController>();
        fishController.SetFishName(newFish.name);
        fishController.SetFishId(newFish.id);
        fishController.SetHungerLevel(newFish.hungerLevel);
        fishController.SetSocialLevel(newFish.socialLevel);

        
        NamedSprite spritePair = fishTemplateProvider.GetSpritePair(newFish.template);
        fishController.SetFishTemplate(spritePair);
        if (newFish.sprite != "") fishController.SetFishSprite(newFish.sprite);
    }

    public void DeleteFish(FishController fish)
    {
        StartCoroutine(DeleteFishCoroutine(fish));
    }

    void InstantiateAllFish()
    {
        if (fishList.Count > 0)
        {
            foreach (var fish in fishList)
            {
                InstantiateFish(fish);
            }
        }
    }

    private IEnumerator DeleteFishCoroutine(FishController fishController)
    {
        var task = FishDeleteAsync(fishController.fishId);
        while (!task.IsCompleted)
        {
            yield return null;
        }

        if (task.Exception != null)
        {
            Debug.LogError("Failed to delete fish: " + task.Exception.InnerException.Message);
        }
        else
        {
            GameObject fishGo = fishController.gameObject;
            Destroy(fishGo);
        }
    }

    private async Task<bool> FishDeleteAsync(int fishId)
    {
        return await fishApi.FishDelete(fishId);
    }
}