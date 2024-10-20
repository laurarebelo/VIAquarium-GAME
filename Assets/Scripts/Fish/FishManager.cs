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
    public List<GameObject> fishInScene = new List<GameObject>();
    private int z;
    public TMP_InputField fishNameField;


    void Start()
    {
        _ = GetAllFish();
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
        fishController.SetFishColor(Utils.GetRandomColor());
        fishController.SetFishId(newFish.id);
        fishController.SetHungerLevel(newFish.hungerLevel);
        fishInScene.Add(newFishGo);
    }

    public async Task ResetFish()
    {
        DestroyAllFish();
        await GetAllFish();
    }

    public void DeleteFish(FishController fish)
    {
        StartCoroutine(DeleteFishCoroutine(fish));
    }

    void DestroyAllFish()
    {
        foreach (var fishGameObject in fishInScene)
        {
            Destroy(fishGameObject);
        }
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
    
    public void SubmitFish()
    {
        StartCoroutine(SubmitFishCoroutine());
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

    private IEnumerator SubmitFishCoroutine()
    {
        string fishName = fishNameField.text;

        FishGetObject fishObject;
        var task = FishPostAsync(fishName);
        while (!task.IsCompleted)
        {
            yield return null;
        }

        if (task.Exception != null)
        {
            Debug.LogError("Failed to submit fish: " + task.Exception.InnerException.Message);
        }
        else
        {
            fishObject = task.Result;
            SpawnFishAsync(fishObject);
        }

        fishNameField.text = "";
    }

    private async Task<bool> FishDeleteAsync(int fishId)
    {
        return await fishApi.FishDelete(fishId);
    }

    private async Task<FishGetObject> FishPostAsync(string fishName)
    {
        return await fishApi.FishPost(fishName);
    }

    private void SpawnFishAsync(FishGetObject newFish)
    {
        InstantiateFish(newFish);
    }
}
