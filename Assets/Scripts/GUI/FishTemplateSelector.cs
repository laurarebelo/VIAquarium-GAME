using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishTemplateSelector : MonoBehaviour
{
    public Image outlineSprite;
    public Button button;
    public TMP_Text fishNameText;
    public NamedSprite namedSprite;
    private FishTemplateProvider fishTemplateProvider;
    public Color deselectedColor;
    public Color selectedColor;

    private void Awake()
    {
        button.onClick.AddListener(OnTemplateSelected);
        fishTemplateProvider = GameObject.Find("FishTemplateProvider").GetComponent<FishTemplateProvider>();
        Deselect();
    }

    public void SetTemplate(NamedSprite template)
    {
        namedSprite = template;
        fishNameText.text = template.name;
        outlineSprite.sprite = template.outlineSprite;
        AdjustSizes();
    }

    public string TemplateName()
    {
        return namedSprite.name;
    }

    private void AdjustSizes()
    {
        if (namedSprite != null)
        {
            if (namedSprite.outlineSprite.texture.width == 64)
            {
                RectTransform buttonRectTransform = button.GetComponent<RectTransform>();
                buttonRectTransform.sizeDelta = new Vector2(300, 150);
                RectTransform imageRectTransform = outlineSprite.GetComponent<RectTransform>();
                imageRectTransform.sizeDelta = new Vector2(300, 150);
            }
        }
    }

    private void OnTemplateSelected()
    {
        if (namedSprite != null)
        {
            fishTemplateProvider.SelectTemplate(this);
        }
    }

    public void Select()
    {
        button.image.color = selectedColor;
    }

    public void Deselect()
    {
        button.image.color = deselectedColor;
    }
}