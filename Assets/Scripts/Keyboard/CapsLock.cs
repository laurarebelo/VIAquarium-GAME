using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsLock : MonoBehaviour
{
    public bool isCapsLockOn = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
    }

    public void ToggleCapsLock()
    {
        isCapsLockOn = !isCapsLockOn;
        UpdateButtonTexts();
        audioSource.Play();
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