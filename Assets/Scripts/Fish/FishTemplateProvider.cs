using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FishTemplateProvider : MonoBehaviour
{
    public UnityEvent<bool> OnTemplateSelectionChanged;

    
    public List<NamedSprite> namedSprites;
    private Dictionary<string, NamedSprite> namedSpritesDictionary;
    public GameObject selectorPrefab;
    public FishTemplateSelector selectedTemplate;
    public GameObject allFishTemplatesParent;
    public GameObject addFishCanvas;

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

    public void ToggleCanvasVisibility(bool? active = null)
    {
        GameObject screen = addFishCanvas;
        if (active == null)
        {
            active = !screen.activeSelf;
        }
        screen.SetActive(active.Value);
        if (!active.Value)
        {
            DeselectTemplate();
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

    public void SelectTemplate(FishTemplateSelector template)
    {
        DeselectTemplate();
        selectedTemplate = template;
        selectedTemplate.Select();
        OnTemplateSelectionChanged.Invoke(true);
    }

    public void DeselectTemplate()
    {
        if (selectedTemplate)
        {
            selectedTemplate.Deselect();
        }
        selectedTemplate = null;
        OnTemplateSelectionChanged.Invoke(false);
    }
}