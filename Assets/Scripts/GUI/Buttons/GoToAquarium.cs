using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToAquarium : MonoBehaviour
{
    public void OnButtonClick()
    {
        SceneManager.LoadScene("MainAquarium");
    }
}