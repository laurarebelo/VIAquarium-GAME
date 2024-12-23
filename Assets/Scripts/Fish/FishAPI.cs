using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

namespace Model
{
    public class FishAPI : MonoBehaviour
    {
        private static FishAPI instance;
        private string url = "http://localhost:5296/api/fish";

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(instance.gameObject);
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        public static FishAPI Instance => instance;

        public async Task<FishGetObject> FishPost(FishPostObject fishPostObject)
        {
            string json = JsonUtility.ToJson(fishPostObject);
            using (UnityWebRequest www = UnityWebRequest.Post(url, json, "application/json"))
            {
                var operation = www.SendWebRequest();
                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(www.error);
                    return null;
                }
                else
                {
                    string jsonResponse = www.downloadHandler.text;
                    FishGetObject fishObject = JsonUtility.FromJson<FishGetObject>(jsonResponse);
                    return fishObject;
                }
            }
        }

        public Task<List<FishGetObject>> GetAllFishAlive()
        {
            return FetchFishData<FishGetObject>("alive");
        }

        public Task<List<DeadFishGetObject>> GetDeadFish([CanBeNull] string sortBy, [CanBeNull] string searchName,
            int? startIndex, int? endIndex)
        {
            string urlAddon = "dead";

            bool firstParam = true;

            if (sortBy != null)
            {
                urlAddon += $"?sortBy={sortBy}";
                firstParam = false;
            }

            if (searchName != null)
            {
                urlAddon += firstParam ? $"?searchName={searchName}" : $"&searchName={searchName}";
                firstParam = false;
            }

            if (startIndex != null)
            {
                urlAddon += firstParam ? $"?startIndex={startIndex}" : $"&startIndex={startIndex}";
                firstParam = false;
            }

            if (endIndex != null)
            {
                urlAddon += firstParam ? $"?endIndex={endIndex}" : $"&endIndex={endIndex}";
            }

            return FetchFishData<DeadFishGetObject>(urlAddon);
        }


        private async Task<List<T>> FetchFishData<T>(string fishType)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get($"{url}/{fishType}"))
            {
                var asyncOperation = webRequest.SendWebRequest();
                while (!asyncOperation.isDone)
                {
                    await Task.Yield();
                }

                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Error fetching fish data: {webRequest.error}");
                    return new List<T>();
                }

                try
                {
                    string jsonResponse = webRequest.downloadHandler.text;
                    string[] eachFishJson = ParseFishListResponse(jsonResponse);
                    List<T> fishList = new List<T>();

                    foreach (var fishJson in eachFishJson)
                    {
                        T fishObject = JsonUtility.FromJson<T>(fishJson);
                        fishList.Add(fishObject);
                    }

                    return fishList;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error parsing fish data: {ex.Message}");
                    return new List<T>();
                }
            }
        }

        string[] ParseFishListResponse(string jsonResponse)
        {
            if (string.IsNullOrEmpty(jsonResponse) || jsonResponse == "[]")
            {
                return Array.Empty<string>();
            }

            string trimmedJson = jsonResponse.Trim(new char[] { '[', ']' });
            string[] jsonObjects = trimmedJson.Split(new string[] { "},{" }, StringSplitOptions.None);
            for (int i = 0; i < jsonObjects.Length; i++)
            {
                jsonObjects[i] = (i >= 0 ? "{" : "") + jsonObjects[i].Trim(new char[] { '{', '}' }) +
                                 (i <= jsonObjects.Length - 1 ? "}" : "");
            }

            return jsonObjects;
        }

        private async Task<T> UploadFishNeed<T>(int fishAffectedId, string needType, int needPoints) where T : class
        {
            if (needType != "hunger" && needType != "social")
            {
                Debug.Log($"Tried to UploadFishNeed with invalid need type: {needType}");
                return null;
            }

            string fullUrl = url + $"/{fishAffectedId}/{needType}";
            NeedPatchObject needPatchObject = new NeedPatchObject(needPoints);
            string jsonBody = JsonUtility.ToJson(needPatchObject);

            using (UnityWebRequest www = new UnityWebRequest(fullUrl, "PATCH"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
                www.uploadHandler = new UploadHandlerRaw(bodyRaw);
                www.SetRequestHeader("Content-Type", "application/json");
                www.downloadHandler = new DownloadHandlerBuffer();

                var operation = www.SendWebRequest();
                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(www.error);
                    return null;
                }
                else
                {
                    string jsonResponse = www.downloadHandler.text;
                    // Deserialize based on the type parameter
                    if (typeof(T) == typeof(FishFedResponse))
                    {
                        FishFedResponse responseObject = JsonUtility.FromJson<FishFedResponse>(jsonResponse);
                        return responseObject as T;
                    }
                    else if (typeof(T) == typeof(FishPetResponse))
                    {
                        FishPetResponse responseObject = JsonUtility.FromJson<FishPetResponse>(jsonResponse);
                        return responseObject as T;
                    }
                    else
                    {
                        Debug.LogError("Unsupported response type.");
                        return null;
                    }
                }
            }
        }

        public async Task<List<FishOnlyNeeds>> GetAliveFishNeeds()
        {
            string fullUrl = $"{url}/alive/needs";
            using (UnityWebRequest www = UnityWebRequest.Get(fullUrl))
            {
                var operation = www.SendWebRequest();
                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (www.result == UnityWebRequest.Result.ConnectionError ||
                    www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Error getting alive fish needs: {www.error}");
                }

                string responseJson = www.downloadHandler.text;
                string[] eachFishNeedsJson = ParseFishListResponse(responseJson);
                List<FishOnlyNeeds> fishNeedList = new List<FishOnlyNeeds>();

                foreach (var fishJson in eachFishNeedsJson)
                {
                    FishOnlyNeeds fishNeedObject = JsonUtility.FromJson<FishOnlyNeeds>(fishJson);
                    fishNeedList.Add(fishNeedObject);
                }

                return fishNeedList;
            }
        }

        public async Task<FishFedResponse> UploadFishFeed(int fishAffectedId, int hungerPoints)
        {
            return await UploadFishNeed<FishFedResponse>(fishAffectedId, "hunger", hungerPoints);
        }

        public async Task<FishPetResponse> UploadFishPet(int fishAffectedId, int pettingPoints)
        {
            return await UploadFishNeed<FishPetResponse>(fishAffectedId, "social", pettingPoints);
        }

        public async Task<bool> FishDelete(int fishId)
        {
            string fullUrl = $"{url}/{fishId}";

            using (UnityWebRequest www = UnityWebRequest.Delete(fullUrl))
            {
                var operation = www.SendWebRequest();
                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (www.result == UnityWebRequest.Result.ConnectionError ||
                    www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Error deleting fish: {www.error}");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public async Task RespectDeadFish(int fishId, int respectCount)
        {
            string fullUrl = $"{url}/dead/{fishId}/respect?howMuch={respectCount}";
            using (UnityWebRequest www = UnityWebRequest.PostWwwForm(fullUrl, ""))
            {
                var operation = www.SendWebRequest();
                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (www.result == UnityWebRequest.Result.ConnectionError ||
                    www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Error respecting dead fish: {www.error}");
                }
            }
        }

        public async Task<FishGetObject> ReviveDeadFish(int fishId)
        {
            string fullUrl = $"{url}/dead/{fishId}/revive";
            using (UnityWebRequest www = UnityWebRequest.PostWwwForm(fullUrl, ""))
            {
                var operation = www.SendWebRequest();
                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (www.result == UnityWebRequest.Result.ConnectionError ||
                    www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Error reviving dead fish: {www.error}");
                }

                string responseJson = www.downloadHandler.text;
                FishGetObject revivedFish = JsonUtility.FromJson<FishGetObject>(responseJson);
                return revivedFish;
            }
        }
    }
}