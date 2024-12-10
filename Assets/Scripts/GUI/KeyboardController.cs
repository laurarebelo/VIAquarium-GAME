using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;

namespace GUI
{
    public class KeyboardController : MonoBehaviour
    {
        public TMP_InputField textField;

        public void AddLetter(string letter)
        {
            if (textField == null)
            {
                Debug.LogError("Input field is not assigned in KeyboardController.");
                return;
            }
            textField.text += letter;
        }
    }
}