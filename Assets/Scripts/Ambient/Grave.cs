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
    public TMP_Text respectCountText;

    public SpriteRenderer outlineSpriteRenderer;
    public SpriteRenderer colorSpriteRenderer;

    public RectTransform respectParentRT;

    private FishTemplateProvider templateProvider;

    private void Awake()
    {
        templateProvider = GameObject.Find("FishTemplateProvider").GetComponent<FishTemplateProvider>();
    }

    public void InitializeGrave(DeadFishGetObject deadFishGetObject)
    {
        fishNameText.text = deadFishGetObject.name;
        birthDateText.text = GetDateMiniString(deadFishGetObject.dateOfBirth);
        deathDateText.text = GetDateMiniString(deadFishGetObject.dateOfDeath);
        respectCountText.text = deadFishGetObject.respectCount.ToString();
        AdjustPosXDependingOnRespectCountDigits();
        outlineSpriteRenderer.sprite = templateProvider.GetSpritePair(deadFishGetObject.template).outlineSprite;
        colorSpriteRenderer.sprite = Utils.GetSpriteFromEncodedString(deadFishGetObject.sprite);
        colorSpriteRenderer.color = Color.white;
    }

    private void AdjustPosXDependingOnRespectCountDigits()
    {
        float newX = respectCountText.text.Length switch
        {
            1 => 0f,
            2 => -0.0925f,
            3 => -0.319125f,
            4 => -0.446775f,
            _ => respectParentRT.localPosition.x
        };

        respectParentRT.localPosition = new Vector3(newX, respectParentRT.localPosition.y, respectParentRT.localPosition.z);
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
