using System.Collections.Generic;
using System.Threading.Tasks;
using Model;
using UnityEngine;
using TMPro;

public class GraveManager : MonoBehaviour
{
    public GameObject gravePrefab;
    public Transform graveParent;
    public FishAPI fishApi;
    public GameObject loadingScreen;


    private FishTemplateProvider templateProvider;

    private void Start()
    {
        templateProvider = GameObject.Find("FishTemplateProvider").GetComponent<FishTemplateProvider>();
        InitializeGraves();
    }

    public async void InitializeGraves()
    {
        if (FishStore.Instance.HasStoredDeadFish())
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

    private void InstantiateDeadFishList(List<DeadFishGetObject> allFish)
    {
        foreach (var fish in allFish)
        {
            InstantiateGrave(fish);
        }
    }

    private void InstantiateGrave(DeadFishGetObject deadFishGetObject)
    {
        GameObject graveInstance = Instantiate(gravePrefab, graveParent);
        Grave graveComponent = graveInstance.GetComponent<Grave>();
        graveComponent.InitializeGrave(deadFishGetObject);
    }

    void ShowLoadingScreen(bool show)
    {
        loadingScreen.SetActive(show);
    }
}