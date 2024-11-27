using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorManager : MonoBehaviour
{
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private GameObject overlay;

    private TextMeshProUGUI errorMessageText;
    private void Awake()
    {
        if (errorPanel != null)
        {
            errorMessageText = errorPanel.GetComponentInChildren<TextMeshProUGUI>();
            if (errorMessageText == null)
            {
                Debug.LogError("TextMeshProUGUI component not found in errorPanel.");
            }
        }
        else
        {
            Debug.LogError("ErrorPanel is not assigned.");
        }

        if (errorPanel != null)
            errorPanel.SetActive(false);
        
        if (overlay != null)
            overlay.SetActive(false);
    }


    public void ShowError(string message)
    {
        if (errorMessageText is not null)
        {
            errorMessageText.text = message;
            errorPanel.SetActive(true);
            
            if (overlay is not null)
                overlay.SetActive(true);
        }
        else
        {
            Debug.LogError("ErrorMessageText is not assigned!");
        }
    }

    public void CloseError()
    {
        if (errorPanel != null)
        {
            errorPanel.SetActive(false);
        }
        
        if (overlay != null)
            overlay.SetActive(false);
    }

}