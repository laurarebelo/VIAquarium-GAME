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
    private FishDeath fishDeath;

    void Start()
    {
        fishDeath = GetComponent<FishDeath>();
        Deselect();
        UpdateText();
    }

    public void SetHungerLevel(int level)
    {
        if (level < 0) level = 0;
        if (level == 0)
        {
            fishDeath.Die();
            FishStore.Instance.RemoveFish(fishId);
        }

        if (level > 100) level = 100;
        hungerLevel = level;
        FishStore.Instance.UpdateStoredFishHunger(fishId, level);
    }

    public void SetSocialLevel(int level)
    {
        if (level < 0) level = 0;
        if (level == 0)
        {
            fishDeath.Die();
            FishStore.Instance.RemoveFish(fishId);
        }

        if (level > 100) level = 100;
        socialLevel = level;
        FishStore.Instance.UpdateStoredFishSocial(fishId, level);
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
        float offset = FishTemplateProvider.GetPosYForName(fishName);
        var sizeDelta = fishNameText.rectTransform.sizeDelta;
        sizeDelta.y = height;
        fishNameText.rectTransform.sizeDelta = sizeDelta;
        Vector3 newRectPos = fishNameText.rectTransform.localPosition;
        newRectPos.y = offset;
        fishNameText.rectTransform.localPosition = newRectPos;
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
        Sprite sprite = Utils.GetSpriteFromEncodedString(encodedSprite);
        fishColorSpriteRenderer.color = Color.white;
        fishColorSpriteRenderer.sprite = sprite;
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