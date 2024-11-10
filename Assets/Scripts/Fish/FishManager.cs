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
    
    void Start()
    {
        _ = GetAllFish();
        fishTemplateProvider = GameObject.Find("FishTemplateProvider").GetComponent<FishTemplateProvider>();
    }

    async Task GetAllFish()
    {
        fishList = await fishApi.FishGetAll();
        InstantiateAllFish();
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
        
        NamedSprite spritePair = fishTemplateProvider.GetSpritePair(newFish.template);
        fishController.SetFishTemplate(spritePair);
        if (newFish.sprite != "") fishController.SetFishSprite(newFish.sprite);
        else fishController.SetFishColor(Utils.GetRandomColor());
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