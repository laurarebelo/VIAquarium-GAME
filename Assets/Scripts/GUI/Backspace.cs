using UnityEngine;

namespace GUI
{
    public class BackspaceKey : MonoBehaviour
    {
        private KeyboardController keyboardController;

        void Start()
        {
            keyboardController = GetComponentInParent<KeyboardController>();
            if (keyboardController == null)
            {
                Debug.LogError("KeyboardController not found in parent keyboard!");
            }
        }

        public void OnBackspacePressed()
        {
            if (keyboardController != null)
            {
                keyboardController.RemoveLetterAtCaret();
            }
        }
    }
}