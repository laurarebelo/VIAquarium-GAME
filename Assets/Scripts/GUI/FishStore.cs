using System.Collections.Generic;
using UnityEngine;

public class FishStore : MonoBehaviour
{
    private static FishStore instance;
    private List<FishGetObject> storedFish;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            storedFish = new List<FishGetObject>();
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

    public void StoreFish(FishGetObject fish)
    {
        storedFish.Add(fish);
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

    public List<FishGetObject> GetStoredFish()
    {
        return new List<FishGetObject>(storedFish);
    }

    public bool HasStoredFish()
    {
        return storedFish.Count > 0;
    }
}