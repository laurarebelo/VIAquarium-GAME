using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model;
using UnityEngine;

public class DeadFishManager : MonoBehaviour
{
    private FishTemplateProvider fishTemplateProvider;
    private FishAPI fishApi;
    public GameObject loadingScreen;
    public Transform foregroundParentGo;
    public GameObject gravePrefab;

    void Start()
    {
        fishTemplateProvider = GameObject.Find("FishTemplateProvider").GetComponent<FishTemplateProvider>();
        fishApi = GameObject.Find("FishApi").GetComponent<FishAPI>();
        _ = InitializeDeadFish();
    }

    async Task InitializeDeadFish()
    {
        if (FishStore.Instance.HasStoredFish())
        {
            InstantiateDeadFishList(FishStore.Instance.GetStoredDeadFish());
        }
        else
        {
            ShowLoadingScreen(true);
            var allFish = await fishApi.GetAllFishDead();
            FishStore.Instance.StoreDeadFishList(allFish);
            InstantiateDeadFishList(allFish);
            ShowLoadingScreen(false);
        }
    }
    
    void ShowLoadingScreen(bool show)
    {
        loadingScreen.SetActive(show);
    }

    Vector3 GetGravePosition(int index)
    {
        float x = -6.85f + index * 4;
        return new Vector3(x, -0.59f, 10);
    }

    void InstantiateDeadFish(DeadFishGetObject deadFish, int index)
    {
        Debug.Log("Called instantiate dead fish");
        Vector3 position = GetGravePosition(index);
        GameObject newDeadFishGo = Instantiate(gravePrefab, position, Quaternion.identity, foregroundParentGo);
        Grave graveController = newDeadFishGo.GetComponent<Grave>();
        graveController.InitializeGrave(deadFish);
    }

    void InstantiateDeadFishList(List<DeadFishGetObject> deadFishList)
    {
        for (int i = 0; i < deadFishList.Count; i++)
        {
            Debug.Log($"Instantiating dead fish index {i} name {deadFishList[i].name}");
            InstantiateDeadFish(deadFishList[i], i);
        }
    }
}
