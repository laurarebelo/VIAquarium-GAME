using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Utils
{
    public static float margin = 100f;

    public static Vector3 GetRandomPosition(Vector3 topLeftBoundary, Vector3 bottomRightBoundary)
    {
        float randomX = Random.Range(topLeftBoundary.x, bottomRightBoundary.x);
        float randomY = Random.Range(bottomRightBoundary.y, topLeftBoundary.y);
        Vector3 randomPosition = new Vector3(randomX, randomY, 0);
        return randomPosition;
    }

    public static Sprite GetSpriteFromEncodedString(string encodedSprite)
    {
        byte[] imageBytes = System.Convert.FromBase64String(encodedSprite);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(imageBytes))
        {
            texture.filterMode = FilterMode.Point;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            return sprite;
        }

        throw new Exception("Something went wrong when getting sprite from encoded string...");
    }

    public static Color GetRandomColor()
    {
        float minValue = 0.2f;

        float r = Random.Range(minValue, 1f);
        float g = Random.Range(minValue, 1f);
        float b = Random.Range(minValue, 1f);

        return new Color(r, g, b);
    }
    
    public static T GetRandomItem<T>(List<T> items)
    {
        if (items == null || items.Count == 0)
        {
            Debug.LogWarning("The item list is null or empty.");
            return default;
        }

        int randomIndex = Random.Range(0, items.Count);
        return items[randomIndex];
    }
    
    public static T[] FromJsonArray<T>(string json)
    {
        string arrayJson = json.Trim(new char[] { '[', ']' }); // Remove brackets
        string[] jsonItems = arrayJson.Split(new string[] { "},{" }, StringSplitOptions.None);
        T[] items = new T[jsonItems.Length];

        for (int i = 0; i < jsonItems.Length; i++)
        {
            jsonItems[i] = (i > 0 ? "{" : "") + jsonItems[i].Trim(new char[] { '{', '}' }) + (i < jsonItems.Length - 1 ? "}" : "");
            items[i] = JsonUtility.FromJson<T>(jsonItems[i]);
        }

        return items;
    }
    
    public static string GetDateMiniString(string dateStringYYYYMMDD)
    {
        string[] parts = dateStringYYYYMMDD.Split('-');
        string year = parts[0].Substring(2);
        string month = parts[1];
        string day = parts[2].Substring(0, 2);
        return $"{day}.{month}.{year}";
    }
}