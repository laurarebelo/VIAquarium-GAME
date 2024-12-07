using System.Collections.Generic;
using UnityEngine;

public class FishStore : MonoBehaviour
{
    private static FishStore instance;
    private List<FishGetObject> storedFish;
    private List<DeadFishGetObject> storedDeadFish;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            storedFish = new List<FishGetObject>();
            storedDeadFish = new List<DeadFishGetObject>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static FishStore Instance => instance;

    public void RemoveFish(int fishId)
    {
        FishGetObject fishToRemove = storedFish.Find(fish => fish.id == fishId);
        if (fishToRemove != null)
        {
            storedFish.Remove(fishToRemove);
        }
    }

    public void RemoveDeadFish(int fishId)
    {
        DeadFishGetObject fishToRemove = storedDeadFish.Find(fish => fish.id == fishId);
        if (fishToRemove != null)
        {
            storedDeadFish.Remove(fishToRemove);
        }
    }

    public void StoreFish(FishGetObject fish)
    {
        storedFish.Add(fish);
    }

    public void StoreDeadFish(DeadFishGetObject fish)
    {
        if (!storedDeadFish.Exists(f => f.id == fish.id))
        {
            storedDeadFish.Add(fish);
            Debug.Log($"Stored dead fish: {fish.name}");
        }
    }

    public void UpdateStoredFishHunger(int fishId, int hungerLevel)
    {
        FishGetObject fishToUpdate = storedFish.Find(fish => fish.id == fishId);
        if (fishToUpdate != null)
        {
            fishToUpdate.hungerLevel = hungerLevel;
            fishToUpdate.lastUpdatedHunger = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }

    public void UpdateStoredFishSocial(int fishId, int socialLevel)
    {
        FishGetObject fishToUpdate = storedFish.Find(fish => fish.id == fishId);
        if (fishToUpdate != null)
        {
            fishToUpdate.socialLevel = socialLevel;
            fishToUpdate.lastUpdatedSocial = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }

    public void StoreFishList(List<FishGetObject> fishList)
    {
        storedFish.AddRange(fishList);
    }

    public void StoreDeadFishList(List<DeadFishGetObject> fishList)
    {
        storedDeadFish.AddRange(fishList);
    }

    public List<FishGetObject> GetStoredFish()
    {
        return new List<FishGetObject>(storedFish);
    }

    public List<DeadFishGetObject> GetStoredDeadFish()
    {
        return new List<DeadFishGetObject>(storedDeadFish);
    }

    public bool HasStoredFish()
    {
        return storedFish.Count > 0;
    }

    public bool HasStoredDeadFish()
    {
        return storedDeadFish.Count > 0;
    }
}
