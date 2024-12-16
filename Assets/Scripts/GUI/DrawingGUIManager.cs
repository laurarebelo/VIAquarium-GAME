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
    public RenderTexture renderTexture;
    public int NameSize = 20;
    
    void Start()
    {
        nameInputField.characterLimit = NameSize;
        errorManager = GameObject.Find("ErrorManager").GetComponent<ErrorManager>();
        submitButton.onClick.AddListener(() => StartCoroutine(SubmitFishCoroutine()));
        backButton.onClick.AddListener(GoBack);
    }

    private void GoBack()
    {
        SceneManager.LoadScene("MainAquarium");
    }

    private IEnumerator SubmitFishCoroutine()
    {
        string fishName = nameInputField.text;
        if (!ValidateName(fishName)) yield break;
        string fishTemplate = FishTemplateProvider.Instance.selectedTemplate.TemplateName();
        string image = SaveTextureAsByteString();
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
        FishTemplateProvider.Instance.DeselectTemplate();
        GoBack();
    }
    
    private async Task<FishGetObject> FishPostAsync(FishPostObject fishPostObject)
    {
        return await FishAPI.Instance.FishPost(fishPostObject);
    }
    
    private string SaveTextureAsByteString()
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
    
    // Validation method to check if the name is not empty and contains only letters and spaces
    bool ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            errorManager.ShowError("Please do not submit an empty name!");
            return false;
        }
        if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
        {
            errorManager.ShowError( name + " is very special, however " +
                                    "NO SPECIAL CHARACTERS are allowed in the name!");
            return false;
        }
        if (name.Length > NameSize)
        {
            errorManager.ShowError($"This shouldn't be possible, but please " +
                                   $"keep your name under {NameSize} characters.");
            return false;
        }
        return true;
    }
}