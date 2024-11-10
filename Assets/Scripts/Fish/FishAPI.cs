using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Model
{
    public class FishAPI : MonoBehaviour
    {
        private string url = "http://localhost:5296/api/fish";

        public async Task<FishGetObject> FishPost(string fishName)
        {
            string fullUrl = url + "?fishName=" + fishName;
            using (UnityWebRequest www = UnityWebRequest.Post(fullUrl, null, "application/json"))
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

        public async Task<List<FishGetObject>> FishGetAll()
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                var asyncOperation = webRequest.SendWebRequest();
                while (!asyncOperation.isDone)
                {
                    await Task.Yield();
                }

                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Error: {webRequest.error}");
                    return new List<FishGetObject>();
                }
                else
                {
                    string jsonResponse = webRequest.downloadHandler.text;
                    string[] eachFishJson = ParseFishListResponse(jsonResponse);
                    List<FishGetObject> fishList = new List<FishGetObject>();
                    foreach (var fishJson in eachFishJson)
                    {
                        FishGetObject fishObject = JsonUtility.FromJson<FishGetObject>(fishJson);
                        fishList.Add(fishObject);
                    }

                    return fishList;
                }
            }
        }

        string[] ParseFishListResponse(string jsonResponse)
        {
            string trimmedJson = jsonResponse.Trim(new char[] { '[', ']' });
            string[] jsonObjects = trimmedJson.Split(new string[] { "},{" }, StringSplitOptions.None);
            for (int i = 0; i < jsonObjects.Length; i++)
            {
                jsonObjects[i] = (i >= 0 ? "{" : "") + jsonObjects[i].Trim(new char[] { '{', '}' }) +
                                 (i <= jsonObjects.Length - 1 ? "}" : "");
            }

            return jsonObjects;
        }

        // note for Bianca:
        // for UploadFishPet i would imagine you need to create a new type
        // in the Model folder called FishPetResponse
        // since the return from the api is slightly different :D

       
        // public async Task<FishPetResponse> UploadFishPet(int fishAffectedId, int hungerPoints)
        // {
        //     return await UploadFishNeed(fishAffectedId, "social", hungerPoints);
        // }

        // public async Task<FishFedResponse> UploadFishFeed(int fishAffectedId, int hungerPoints)
        // {
        //     return await UploadFishNeed(fishAffectedId, "hunger", hungerPoints);
        // }

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

    }
}