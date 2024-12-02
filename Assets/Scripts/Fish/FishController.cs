using TMPro;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public int fishId;
    public string fishName = "...";
    public TMP_Text fishNameText;
    public NamedSprite fishSprites;
    public SpriteRenderer fishOutlineSpriteRenderer;
    public SpriteRenderer fishColorSpriteRenderer;
    public GameObject fishSelectedCircle;
    public GameObject emotionsDisplayGo;
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
    }

    public void SetSocialLevel(int level)
    {
        if (level < 0) level = 0;
        if (level > 100) level = 100;
        socialLevel = level;
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
        AdjustNameHeight();
        AdjustEmotionsPosition();
        ChangeHueDependingOnTime();
    }

    private void AdjustEmotionsPosition()
    {
        Vector3 localPosition = FishTemplateProvider.GetLocalTransformForEmotionsBubble(fishSprites.name);
        emotionsDisplayGo.transform.localPosition = localPosition;
    }

    private void AdjustNameHeight()
    {
        float height = FishTemplateProvider.GetRectTransformHeightForTemplateType(fishSprites.name);
        var sizeDelta = fishNameText.rectTransform.sizeDelta;
        sizeDelta.y = height;
        fishNameText.rectTransform.sizeDelta = sizeDelta;
    }

    private void ChangeHueDependingOnTime()
    {
        SetFishColor(BackgroundManager.GetColorForTimeOfDay());
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
        ChangeHueDependingOnTime();
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