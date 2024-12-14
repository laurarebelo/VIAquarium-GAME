using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;
using TMPro;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public AudioSource bgmAudioSource;
    public FishAPI fishApi;
    private List<FishGetObject> fishList;
    public GameObject fishPrefab;
    private int z;
    private FishTemplateProvider fishTemplateProvider;
    public GameObject loadingScreen;
    private SceneSizeManager sceneSizeManager;

    private List<FishController> allFishControllers = new List<FishController>();


    void Start()
    {
        sceneSizeManager = GameObject.Find("SceneSizeManager").GetComponent<SceneSizeManager>();
        fishTemplateProvider = GameObject.Find("FishTemplateProvider").GetComponent<FishTemplateProvider>();
        _ = InitializeFish();
        StartCoroutine(UpdateFishNeeds());
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
        bgmAudioSource.Play();
        sceneSizeManager.SetBoundsForFishNumber(fishList.Count);
        z = fishList.Count;
        foreach (var fish in fishList)
        {
            InstantiateFish(fish);
        }
    }

public void InstantiateFish(FishGetObject newFish)
    {
        Vector3 position = Utils.GetRandomPosition(sceneSizeManager.MinBounds.position, sceneSizeManager.MaxBounds.position);
        position.z = z;
        z--;
        GameObject newFishGo = Instantiate(fishPrefab, position, Quaternion.identity);
        FishController fishController = newFishGo.GetComponent<FishController>();
        fishController.SetFishName(newFish.name);
        fishController.SetFishId(newFish.id);
        fishController.SetHungerLevel(newFish.hungerLevel);
        fishController.SetSocialLevel(newFish.socialLevel);

        
        NamedSprite spritePair = fishTemplateProvider.GetSpritePair(newFish.template);
        fishController.SetFishTemplate(spritePair);
        if (newFish.sprite != "") fishController.SetFishSprite(newFish.sprite);
        allFishControllers.Add(fishController);
    }

    public void DeleteFish(FishController fish)
    {
        StartCoroutine(DeleteFishCoroutine(fish));
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
            allFishControllers.Remove(fishController);
        }
    }

    private async Task<bool> FishDeleteAsync(int fishId)
    {
        return await fishApi.FishDelete(fishId);
    }

    IEnumerator UpdateFishNeeds()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f);
            var updateTask = UpdateFishNeedsAsync();
            yield return new WaitUntil(() => updateTask.IsCompleted);
        }
    }

    async Task UpdateFishNeedsAsync()
    {
        if (allFishControllers.Count == 0) return;
        var fishNeedsList = await fishApi.GetAliveFishNeeds();
        Debug.Log(allFishControllers.Count);
        foreach (var fishController in allFishControllers)
        {
            var fishNeeds = fishNeedsList.FirstOrDefault(f => f.id == fishController.fishId);
            if (fishNeeds != null)
            {
                fishController.SetHungerLevel(fishNeeds.hungerLevel);
                fishController.SetSocialLevel(fishNeeds.socialLevel);
                FishStore.Instance.UpdateStoredFishHunger(fishController.fishId, fishNeeds.hungerLevel);
                FishStore.Instance.UpdateStoredFishSocial(fishController.fishId, fishNeeds.socialLevel);
            }
            else
            {
                allFishControllers.Remove(fishController);
                fishController.gameObject.GetComponent<FishDeath>().Die();
            }
        }
    }

}