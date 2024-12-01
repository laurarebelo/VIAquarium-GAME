using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTextColorChange : MonoBehaviour
{
    public Button button;
    public TMP_Text buttonText;
    public Color disabledColor = Color.gray;
    public Color enabledColor = Color.white;

    void Start()
    {
        if (button != null && buttonText != null)
        {
            UpdateTextColor();
            button.onClick.AddListener(UpdateTextColor);
        }
    }

    void Update()
    {
        UpdateTextColor();
    }

    void UpdateTextColor()
    {
        if (button != null && buttonText != null)
        {
            if (button.interactable)
            {
                buttonText.color = enabledColor;
            }
            else
            {
                buttonText.color = disabledColor;
            }
        }
    }
}
