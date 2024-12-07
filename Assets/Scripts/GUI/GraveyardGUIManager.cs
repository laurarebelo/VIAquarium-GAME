using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GraveyardGUI : MonoBehaviour
{
    public Button AquariumButton;
    
    void Start()
    {
        AquariumButton.onClick.AddListener(GoToAquarium);
    }

    private void GoToAquarium()
    {
        SceneManager.LoadScene("S8-Visuals");
    }
    
}
