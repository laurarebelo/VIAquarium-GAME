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

    public void RemoveDeadFish(int deadFishId)
    {
        DeadFishGetObject deadFishToRemove = storedDeadFish.Find(fish => fish.id == deadFishId);
        if (deadFishToRemove != null)
        {
            storedDeadFish.Remove(deadFishToRemove);
        }
    }
    
    // Alive fish

    public void StoreFish(FishGetObject fish)
    {
        storedFish.Add(fish);
    }
    
    public void StoreFishList(List<FishGetObject> fishList)
    {
        storedFish.AddRange(fishList);
    }
    
    public List<FishGetObject> GetStoredFish()
    {
        return new List<FishGetObject>(storedFish);
    }

    public bool HasStoredFish()
    {
        return storedFish.Count > 0;
    }
    
    // Dead fish

    public void StoreDeadFish(DeadFishGetObject deadFish)
    {
        storedDeadFish.Add(deadFish);
    }

    public void StoreDeadFishList(List<DeadFishGetObject> deadFishList)
    {
        storedDeadFish.AddRange(deadFishList);
    }

    public void SetStoredDeadFish(List<DeadFishGetObject> deadFishList)
    {
        storedDeadFish = new List<DeadFishGetObject>(deadFishList);
    }

    public List<DeadFishGetObject> GetStoredDeadFish()
    {
        return new List<DeadFishGetObject>(storedDeadFish);
    }

    public bool HasDeadStoredFish()
    {
        return storedDeadFish.Count > 0;
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

    public void IncrementDeadFishRespectLevel(int fishId, int respectCount)
    {
        DeadFishGetObject deadFishToUpdate = storedDeadFish.Find(fish => fish.id == fishId);
        {
            if (deadFishToUpdate != null)
            {
                deadFishToUpdate.respectCount += respectCount;
            }
        }
    }
   

    
}