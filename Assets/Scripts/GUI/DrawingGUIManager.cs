using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DrawingGUIManager : MonoBehaviour
{
    private ErrorManager errorManager;
    public Button backButton;

    public Button submitButton;

    public TMP_InputField nameInputField;

    private FishTemplateProvider fishTemplateProvider;

    private FishAPI fishApi;
    public RenderTexture renderTexture;
    private List<FishGetObject> fishList;
    
    public int NameSize = 30;


    // Start is called before the first frame update
    void Start()
    {
        fishTemplateProvider = GameObject.Find("FishTemplateProvider").GetComponent<FishTemplateProvider>();
        fishApi = GameObject.Find("FishApi").GetComponent<FishAPI>();
        errorManager = GameObject.Find("ErrorManager").GetComponent<ErrorManager>();
        
        submitButton.onClick.AddListener(() => StartCoroutine(SubmitFishCoroutine()));
        backButton.onClick.AddListener(GoBack);
        fishList = FishStore.Instance.GetStoredFish();
    }

    private void GoBack()
    {
        SceneManager.LoadScene("S8-Visuals");
    }

    private IEnumerator SubmitFishCoroutine()
    {
        string fishName = nameInputField.text;

        if (!ValidateName(fishName)) yield break;
        string fishTemplate = fishTemplateProvider.selectedTemplate.TemplateName();
        string image = SaveTextureAsPNG();
        FishPostObject fishPostObject = new FishPostObject(fishName, fishTemplate, image);
        var task = FishPostAsync(fishPostObject);
        while (!task.IsCompleted)
        {
            yield return null;
        }

        if (task.Exception != null)
        {
            Debug.LogError("Failed to submit fish: " + task.Exception.InnerException.Message);
        }
        
        FishGetObject fishObject = task.Result;
        FishStore.Instance.StoreFish(fishObject);
        
        nameInputField.text = "";
        fishTemplateProvider.DeselectTemplate();
        GoBack();
    }
    
    private async Task<FishGetObject> FishPostAsync(FishPostObject fishPostObject)
    {
        return await fishApi.FishPost(fishPostObject);
    }
    
    private string SaveTextureAsPNG()
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();
        RenderTexture.active = currentRT;
        byte[] bytes = tex.EncodeToPNG();
        Destroy(tex);
        string byteString = System.Convert.ToBase64String(bytes);
        return byteString;
    }
    
    // Validation method to check if the name contains only letters and is unique
    bool ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || !System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z\s]+$") || name.Length > NameSize)
        {
            errorManager.ShowError( name + " is very special, however NO SPECIAL CHARACTERS are allowed in the name!");

            return false;
        }
        
        foreach (var fishGetObject in fishList)
        {
            if (fishGetObject.name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
            {
                errorManager.ShowError("There is only space for one " + name + " in the tank, make the name UNIQUE!");

                return false;
            }
        }

        return true;
    }
}