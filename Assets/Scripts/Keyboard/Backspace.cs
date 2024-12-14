using UnityEngine;

namespace GUI
{
    public class BackspaceKey : MonoBehaviour
    {
        private KeyboardController keyboardController;
        private AudioSource audioSource;

        void Start()
        {
            keyboardController = GetComponentInParent<KeyboardController>();
            audioSource = GetComponentInParent<AudioSource>();
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
                audioSource.Play();
            }
        }
    }
}