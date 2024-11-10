using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTemplateProvider : MonoBehaviour
{
    public List<NamedSprite> namedSprites;
    private Dictionary<string, NamedSprite> namedSpritesDictionary;
    public GameObject selectorPrefab;
    public FishTemplateSelector selectedTemplate;
    public GameObject allFishTemplatesParent;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        namedSpritesDictionary = new Dictionary<string, NamedSprite>();
        foreach (var namedSprite in namedSprites)
        {
            namedSpritesDictionary[namedSprite.name.ToLower()] = namedSprite;
        }

        if (allFishTemplatesParent)
        {
            InstantiateAllTemplatePrefabs();
        }
        else
        {
            selectedTemplate.SetTemplate(namedSpritesDictionary["default"]);
        }
    }

    private void InstantiateAllTemplatePrefabs()
    {
        foreach (var namedSprite in namedSprites)
        {
            GameObject templateButton = Instantiate(selectorPrefab, allFishTemplatesParent.transform);
            FishTemplateSelector templateSelector = templateButton.GetComponent<FishTemplateSelector>();
            if (templateSelector != null)
            {
                templateSelector.SetTemplate(namedSprite);
            }
        }
    }

    public NamedSprite GetSpritePair(string template)
    {
        return namedSpritesDictionary[template.ToLower()];
    }

    public void SetSelectedTemplate(FishTemplateSelector template)
    {
        DeselectTemplate();
        selectedTemplate = template;
        selectedTemplate.Select();
    }

    public void DeselectTemplate()
    {
        if (selectedTemplate)
        {
            selectedTemplate.Deselect();
        }

        selectedTemplate = null;
    }

}
