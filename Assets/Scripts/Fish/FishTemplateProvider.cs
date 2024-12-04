using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FishTemplateProvider : MonoBehaviour
{
    private static FishTemplateProvider instance;
    public UnityEvent<bool> OnTemplateSelectionChanged;
    public List<NamedSprite> namedSprites;
    private Dictionary<string, NamedSprite> namedSpritesDictionary;
    public GameObject selectorPrefab;
    public FishTemplateSelector selectedTemplate;
    public GameObject allFishTemplatesParent;
    public GameObject addFishCanvas;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
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

    public static float GetPosYForTemplateType(string templateName)
    {
        switch (templateName.ToLower())
        {
            case "jellyfish":
                return 1f;
            case "pufferfish":
            case "angelfish":
            case "anglerfish":
            case "starfish":
                return 0.9f;
            case "default":
            case "clownfish":
                return 0.75f;
            case "sardine":
                return 0.55f;
            default:
                return 0.9f;
        }
    }

    public static Vector3 GetLocalTransformForEmotionsBubble(string templateName)
    {
        switch (templateName.ToLower())
        {
            case "jellyfish":
                return new Vector3(0.9f, 0.9f, 0);
            case "anglerfish":
                return new Vector3(1.05f, 0.4f, 0);
            case "pufferfish":
                return new Vector3(1.05f, 0.7f, 0);
            case "starfish":
                return new Vector3(0.7f, 0.7f, 0);
            default:
                return new Vector3(1.05f, 0.5f, 0);
        }
    }
}