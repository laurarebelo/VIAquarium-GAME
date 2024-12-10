using System.Collections;
using System.Collections.Generic;
using GUI;
using TMPro;
using UnityEngine;

public class GenericKey : MonoBehaviour
{
    [SerializeField] public TMP_Text keyText;
    private KeyboardController keyboardController;

    void Start()
    {
        keyboardController = GetComponentInParent<KeyboardController>();
        if (keyboardController is null)
            Debug.LogError("KeyboardController not found in parent keyboard");
        if (keyText is null)
            Debug.LogError("TMP_Text for key not assigned");
    }

    public void OnKeyPressed()
    {
        if (keyboardController != null && keyText != null)
        {
            keyboardController.AddLetter(keyText.text);
        }
        else
        {
            Debug.LogError("Parent keyboard or button text is not assigned.");
        }
    }
}