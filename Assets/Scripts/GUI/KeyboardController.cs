using UnityEngine;
using TMPro;
using System.Collections;

namespace GUI
{
    public class KeyboardController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float distanceThreshold = 0.01f;
        public TMP_InputField textField;
        private RectTransform rectTransform;

        public float onScreen;
        public float offScreen;

        public bool moveOnXAxis = false;

        private bool isMoving = false;

        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void Toggle()
        {
            if (moveOnXAxis)
            {
                if (Mathf.Abs(rectTransform.anchoredPosition.x - offScreen) < distanceThreshold)
                {
                    Appear();
                }
                else if (Mathf.Abs(rectTransform.anchoredPosition.x - onScreen) < distanceThreshold)
                {
                    Disappear();
                }
            }
            else
            {
                if (Mathf.Abs(rectTransform.anchoredPosition.y - offScreen) < distanceThreshold)
                {
                    Appear();
                }
                else if (Mathf.Abs(rectTransform.anchoredPosition.y - onScreen) < distanceThreshold)
                {
                    Disappear();
                }
            }
        }

        public void Appear()
        {
            if (!isMoving)
            {
                if (moveOnXAxis)
                {
                    StartMove(onScreen);
                }
                else
                {
                    StartMove(onScreen);
                }
            }
        }

        public void Disappear()
        {
            if (moveOnXAxis)
            {
                StartMove(offScreen);
            }
            else
            {
                StartMove(offScreen);
            }
        }

        public void AddLetter(string letter)
        {
            if (textField == null)
            {
                Debug.LogError("Input field is not assigned in KeyboardController.");
                return;
            }

            if (textField.text.Length < 20)
            {
                int caretPosition = textField.caretPosition;
                textField.text = textField.text.Insert(caretPosition, letter);
                textField.caretPosition = caretPosition + 1;
            }
        }

        public void RemoveLetterAtCaret()
        {
            if (textField == null)
            {
                Debug.LogError("Input field is not assigned in KeyboardController.");
                return;
            }

            int caretPosition = textField.caretPosition;
            if (caretPosition > 0)
            {
                textField.text = textField.text.Remove(caretPosition - 1, 1);
                textField.caretPosition = caretPosition - 1;
            }
        }

        private void StartMove(float targetPosition)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothMove(targetPosition));
        }

        private IEnumerator SmoothMove(float targetPosition)
        {
            isMoving = true;

            Vector2 targetPos = moveOnXAxis
                ? new Vector2(targetPosition, rectTransform.anchoredPosition.y)
                : new Vector2(rectTransform.anchoredPosition.x, targetPosition);

            while (Vector2.Distance(rectTransform.anchoredPosition, targetPos) > distanceThreshold)
            {
                rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPos,
                    moveSpeed * Time.deltaTime);
                yield return null;
            }

            rectTransform.anchoredPosition = targetPos;
            isMoving = false;
        }
    }
}