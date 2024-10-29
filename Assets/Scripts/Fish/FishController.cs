using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions; // Import for Regex
using TMPro;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public int fishId;
    public string fishName = "...";
    public TMP_Text fishNameText;
    public SpriteRenderer fishColorSprite;
    public GameObject fishSelectedCircle;
    public int hungerLevel;

    void Start()
    {
        Deselect();
        UpdateText();
    }

    public void SetHungerLevel(int level)
    {
        if (level < 0) level = 0;
        if (level > 100) level = 100;
        hungerLevel = level;
    }

    public void Select()
    {
        fishSelectedCircle.SetActive(true);
    }

    public void Deselect()
    {
        fishSelectedCircle.SetActive(false);
    }

    public void SetFishColor(Color color)
    {
        fishColorSprite.color = color;
    }

    public void SetFishId(int id)
    {
        fishId = id;
    }

    public void SetFishName(string newName)
    {
        if (ValidateName(newName)) 
        {
            fishName = newName;
            UpdateText();
        }
        else
        {
            Debug.LogWarning("Invalid fish name! It should contain only letters.");
        }
    }

    void UpdateText()
    {
        fishNameText.text = fishName;
    }

    bool ValidateName(string name)
    {
        return Regex.IsMatch(name, @"^[a-zA-Z]+$");
    }
}