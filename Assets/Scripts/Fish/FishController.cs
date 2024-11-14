using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class FishController : MonoBehaviour
{
    public int fishId;
    public string fishName = "...";
    public TMP_Text fishNameText;
    public NamedSprite fishSprites;
    public SpriteRenderer fishOutlineSpriteRenderer;
    public SpriteRenderer fishColorSpriteRenderer;
    public GameObject fishSelectedCircle;
    public int hungerLevel;
    
    void Start()
    {
        Deselect();
        UpdateText();
    }

    public void SetHungerLevel(int level)
    {
        if (level < 0) level = 0;
        if (level > 100) level = 100;
        hungerLevel = level;
    }

    public void Select()
    {
        fishSelectedCircle.SetActive(true);
    }

    public void Deselect()
    {
        fishSelectedCircle.SetActive(false);
    }

    public void ChangeToDeadOutline()
    {
        fishOutlineSpriteRenderer.sprite = fishSprites.outlineDeadSprite;
    }
    
    public void SetFishTemplate(NamedSprite sprite)
    {
        fishSprites = sprite;
        fishOutlineSpriteRenderer.sprite = sprite.outlineSprite;
        fishColorSpriteRenderer.sprite = sprite.colorSprite;
    }

    public void SetFishColor(Color color)
    {
        fishColorSpriteRenderer.color = color;
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
            fishColorSpriteRenderer.color = Color.white;
            fishColorSpriteRenderer.sprite = sprite;
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
