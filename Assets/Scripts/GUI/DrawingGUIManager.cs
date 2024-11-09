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
    public Button backButton;

    public Button submitButton;

    public TMP_InputField nameInputField;

    private FishTemplateProvider fishTemplateProvider;

    private FishAPI fishApi;
    public RenderTexture renderTexture;

    // Start is called before the first frame update
    void Start()
    {
        fishTemplateProvider = GameObject.Find("FishTemplateProvider").GetComponent<FishTemplateProvider>();
        fishApi = GameObject.Find("FishApi").GetComponent<FishAPI>();
        submitButton.onClick.AddListener(() => StartCoroutine(SubmitFishCoroutine()));
        backButton.onClick.AddListener(GoBack);
    }

    private IEnumerator SubmitFishCoroutine()
    {
        string fishName = nameInputField.text;
        string fishTemplate = fishTemplateProvider.selectedTemplate.TemplateName();
        string image = SaveTextureAsPNG();
        FishGetObject fishObject;
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

        nameInputField.text = "";
        fishTemplateProvider.DeselectTemplate();
        GoBack();
    }
    
    private async Task<FishGetObject> FishPostAsync(FishPostObject fishPostObject)
    {
        return await fishApi.FishPost(fishPostObject);
    }


    void GoBack()
    {
        SceneManager.LoadScene("S2-FeedFish");
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
}