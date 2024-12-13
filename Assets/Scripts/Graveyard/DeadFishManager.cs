using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model;
using TMPro;
using UnityEngine;

public class DeadFishManager : MonoBehaviour
{
    private FishTemplateProvider fishTemplateProvider;
    private FishAPI fishApi;
    public GameObject loadingScreen;
    public GameObject noFishScreen;
    public Transform foregroundParentGo;
    public GameObject gravePrefab;
    public AudioSource bgmAudioSource;

    private List<GameObject> instantiatedGraves = new List<GameObject>();
    private string sortBy = "lastdied";

    private Task currentTask;
    private bool isTaskRunning = false;
    private CameraReset cameraReset;
    private CameraBounds cameraBounds;

    public TMP_InputField searchInputField;
    private string searchName;


    void Start()
    {
        cameraReset = Camera.main.GetComponent<CameraReset>();
        cameraBounds = Camera.main.GetComponent<CameraBounds>();
        fishTemplateProvider = GameObject.Find("FishTemplateProvider").GetComponent<FishTemplateProvider>();
        fishApi = GameObject.Find("FishApi").GetComponent<FishAPI>();
        if (searchInputField != null)
        {
            searchInputField.onEndEdit.AddListener(async => _ = Search(searchInputField.text));
        }

        _ = InitializeDeadFish();
    }

    public void SearchFromInspector()
    {
        _ = Search(searchInputField.text);
    }

    public async Task Search(string search)
    {
        searchName = search;
        await ReloadFish();
    }

    private async Task Sort(string by)
    {
        sortBy = by;
        await ReloadFish();
    }

    private async Task ReloadFish()
    {
        cameraReset.ResetCamera();
        noFishScreen.SetActive(false);
        ShowLoadingScreen(true);
        var allFish = await fishApi.GetDeadFish(sortBy, searchName, null, 32);
        InstantiateDeadFishList(allFish);
        ShowLoadingScreen(false);
        await LoadAdditionalDeadFish();
    }

    public async Task SortByLastDied()
    {
        await Sort("lastdied");
    }

    public async Task SortByMostRespect()
    {
        await Sort("mostrespect");
    }

    public async Task SortByMostDaysLived()
    {
        await Sort("mostdayslived");
    }


    async Task InitializeDeadFish()
    {
        if (FishStore.Instance.HasDeadStoredFish())
        {
            InstantiateDeadFishList(FishStore.Instance.GetStoredDeadFish());
        }
        else
        {
            ShowLoadingScreen(true);
            var allFish = await fishApi.GetDeadFish(sortBy, null, null, 32);
            FishStore.Instance.StoreDeadFishList(allFish);
            InstantiateDeadFishList(allFish);
            ShowLoadingScreen(false);
            _ = LoadAdditionalDeadFish(true);
        }
        bgmAudioSource.Play();
    }

    async Task LoadAdditionalDeadFish(bool store = false)
    {
        var additionalFish = await fishApi.GetDeadFish(sortBy, searchName, null, 100);
        if (additionalFish.Count > 0)
        {
            if (store) FishStore.Instance.SetStoredDeadFish(additionalFish);
            InstantiateDeadFishList(additionalFish);
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
        Vector3 position = GetGravePosition(index);
        GameObject newDeadFishGo = Instantiate(gravePrefab, position, Quaternion.identity, foregroundParentGo);
        Grave graveController = newDeadFishGo.GetComponent<Grave>();
        graveController.InitializeGrave(deadFish);
        instantiatedGraves.Add(newDeadFishGo);
    }

    void InstantiateDeadFishList(List<DeadFishGetObject> deadFishList)
    {
        foreach (var grave in instantiatedGraves)
        {
            Destroy(grave);
        }

        instantiatedGraves.Clear();
        if (deadFishList.Count == 0)
        {
            noFishScreen.SetActive(true);
            return;
        }
        for (int i = 0; i < deadFishList.Count; i++)
        {
            InstantiateDeadFish(deadFishList[i], i);
        }

        cameraBounds.MaxX = GetGravePosition(deadFishList.Count - 1).x - 3;
    }
}