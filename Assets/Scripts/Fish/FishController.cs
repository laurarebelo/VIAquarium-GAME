using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public int fishId;
    public string fishName = "...";
    public TMP_Text fishNameText;
    public SpriteRenderer fishOutlineSprite;
    public SpriteRenderer fishColorSprite;
    public GameObject fishSelectedCircle;
    public int hungerLevel;
    public int socialLevel;

    
    private FishEmotions fishEmotions;

    void Start()
    {
        fishEmotions = GetComponent<FishEmotions>();
        Deselect();
        UpdateText();
    }

    public void SetHungerLevel(int level)
    {
        if (level < 0) level = 0;
        if (level > 100) level = 100;
        hungerLevel = level;
       // fishEmotions.UpdateLowEmotion();
    }

    public void SetSocialLevel(int level)
    {
        if (level < 0) level = 0;
        if (level > 100) level = 100;
        socialLevel = level;
       // fishEmotions.UpdateLowEmotion();
    }
    public void Select()
    {
        fishSelectedCircle.SetActive(true);
    }

    public void Deselect()
    {
        fishSelectedCircle.SetActive(false);

    }
    
    public void SetFishTemplate(NamedSprite sprite)
    {
        fishOutlineSprite.sprite = sprite.outlineSprite;
        fishColorSprite.sprite = sprite.colorSprite;
    }

    public void SetFishColor(Color color)
    {
        fishColorSprite.color = color;
    }

    public void SetFishSprite(string encodedSprite)
    {
        byte[] imageBytes = System.Convert.FromBase64String(encodedSprite);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(imageBytes))
        {
            texture.filterMode = FilterMode.Point;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            fishColorSprite.color = Color.white;
            fishColorSprite.sprite = sprite;
        }
        else
        {
            Debug.LogError("Failed to load image from Base64 string.");
        }
    }

    public void SetFishId(int id)
    {
        fishId = id;
    }

    public void SetFishName(string newName)
    {
        fishName = newName;
        UpdateText();
    }

    void UpdateText()
    {
        fishNameText.text = fishName;
    }
}
