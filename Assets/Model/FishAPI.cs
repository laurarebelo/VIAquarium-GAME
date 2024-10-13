using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Model
{
    public class FishAPI : MonoBehaviour
    {
        private string url = "http://viaquarium-api-env.eba-dcz7rmnw.eu-north-1.elasticbeanstalk.com/api/fish";

        public async Task<FishGetObject> FishPost(string fishName)
        {
            return await UploadFish(fishName);
        }

        public async Task<List<FishGetObject>> FishGet()
        {
            return await DownloadFish();
        }

        private async Task<List<FishGetObject>> DownloadFish()
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

        public async Task<FishGetObject> UploadFish(string fishName)
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
                    Debug.Log("Fish upload complete!");
                    string jsonResponse = www.downloadHandler.text;
                    FishGetObject fishObject = JsonUtility.FromJson<FishGetObject>(jsonResponse);
                    return fishObject;
                }
            }
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
                    Debug.Log("Fish deletion complete!");
                    return true;
                }
            }
        }

    }
}