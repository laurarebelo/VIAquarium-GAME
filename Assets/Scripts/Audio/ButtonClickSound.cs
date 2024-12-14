using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour, IPointerClickHandler
{
    private static ButtonClickSound instance;
    private AudioSource audioSource;
    private Button button;
    private TMP_Dropdown TMPDropdown;

    void Start()
    {
        TMPDropdown = GetComponent<TMP_Dropdown>();
        audioSource = GameObject.Find("ButtonClickSound").GetComponent<AudioSource>();
        if (TMPDropdown)
        {
            TMPDropdown.onValueChanged.AddListener(_ => audioSource.Play());
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        audioSource.Play();
    }

    public void Play()
    {
        audioSource.Play();
    }
}