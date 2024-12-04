using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Grave : MonoBehaviour
{
    public TMP_Text fishNameText;
    public TMP_Text birthDateText;
    public TMP_Text deathDateText;

    public SpriteRenderer outlineSpriteRenderer;
    public SpriteRenderer colorSpriteRenderer;

    private FishTemplateProvider templateProvider;

    private void Start()
    {
        templateProvider = GameObject.Find("FishTemplateProvider").GetComponent<FishTemplateProvider>();
        DeadFishGetObject testObj = new DeadFishGetObject(44, "blood", "2024-12-01T00:00:00", "2024-12-03T00:00:00", 2,
            0, "Loneliness", "Angelfish",
            "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAABKElEQVRYCe2WSw7CMAxEKYIr9P7n4wosiryw5Iy/catmQzf51DPzYlpgO47jsfJ6rgyn7D/A8g68qs/Ad9/Np/X9+WxVD6tum3kLPAgy7oIMH0EUYNHLPdJ29AMAGUYm3VNKUJwrACyYXc92YgDgE0ZdqAJVPQaAzLxqyj6VehcAxbjmkGzMdC6ANM5MZK01j/TqeyAqtsxn9vgZk5pSB6TgzNw63K0AFrwCsNpkCbt72AUFgAXdoKpOAVSF3Trs8K0AGE6HSAEsUef0no8LQAIW8dgJJk2kdwEwLDLBWrnOdGUAMs3MZHC1fvhPyK+gF8T3MQjXnh7raD0A0MaMmOrl1dGqHyNpKOfR6TvB7K06wDfk6IWfCWb/cgdYcPU49RZcHU5+ywF+X5VxW1xakyIAAAAASUVORK5CYII=");
        InitializeGrave(testObj);
    }

    public void InitializeGrave(DeadFishGetObject deadFishGetObject)
    {
        fishNameText.text = deadFishGetObject.name;
        birthDateText.text = GetDateMiniString(deadFishGetObject.dateOfBirth);
        deathDateText.text = GetDateMiniString(deadFishGetObject.dateOfDeath);
        outlineSpriteRenderer.sprite = templateProvider.GetSpritePair(deadFishGetObject.template).outlineSprite;
        colorSpriteRenderer.sprite = Utils.GetSpriteFromEncodedString(deadFishGetObject.sprite);
        colorSpriteRenderer.color = Color.white;
    }

    public string GetDateMiniString(string dateStringYYYYMMDD)
    {
        string[] parts = dateStringYYYYMMDD.Split('-');
        string year = parts[0].Substring(2);
        string month = parts[1];
        string day = parts[2].Substring(0, 2);
        return $"{day}.{month}.{year}";
    }
}
