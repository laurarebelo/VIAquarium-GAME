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
    }

    public string TemplateName()
    {
        return namedSprite.name;
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