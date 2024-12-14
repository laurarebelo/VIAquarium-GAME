using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishTemplateSelector : MonoBehaviour
{
    public Image outlineSprite;
    public Image colorSprite;
    public Button button;
    public TMP_Text fishNameText;
    public NamedSprite namedSprite;
    private FishTemplateProvider fishTemplateProvider;
    public GameObject highlightImage;

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
        colorSprite.sprite = template.defaultSprite;
        colorSprite.color = Color.white;
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
        highlightImage.SetActive(true);
    }

    public void Deselect()
    {
        highlightImage.SetActive(false);
    }
}