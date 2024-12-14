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
    public int deadFishId;
    public DeadFishGetObject deadFish;

    private void Awake()
    {
        templateProvider = GameObject.Find("FishTemplateProvider").GetComponent<FishTemplateProvider>();
    }

    public void InitializeGrave(DeadFishGetObject deadFishGetObject)
    {
        deadFish = deadFishGetObject;
        fishNameText.text = deadFishGetObject.name;
        birthDateText.text = Utils.GetDateMiniString(deadFishGetObject.dateOfBirth);
        deathDateText.text = Utils.GetDateMiniString(deadFishGetObject.dateOfDeath);
        respectCountText.text = deadFishGetObject.respectCount.ToString();
        AdjustPosXDependingOnRespectCountDigits();
        outlineSpriteRenderer.sprite = templateProvider.GetSpritePair(deadFishGetObject.template).outlineSprite;
        colorSpriteRenderer.sprite = Utils.GetSpriteFromEncodedString(deadFishGetObject.sprite);
        colorSpriteRenderer.color = Color.white;
        deadFishId = deadFishGetObject.id;
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
    
}
