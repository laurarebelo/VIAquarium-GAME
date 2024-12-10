using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsLock : MonoBehaviour
{
    public bool isCapsLockOn = false;

    public void ToggleCapsLock()
    {
        isCapsLockOn = !isCapsLockOn;
        UpdateButtonTexts();
    }

    private void UpdateButtonTexts()
    {
        GenericKey[] keyboardButtons = FindObjectsOfType<GenericKey>();

        foreach (var key in keyboardButtons)
        {
            if (key == null || key.keyText == null) continue;

            key.keyText.text = isCapsLockOn
                ? key.keyText.text.ToUpper()
                : key.keyText.text.ToLower();
        }
    }
}