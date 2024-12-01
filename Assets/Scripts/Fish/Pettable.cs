using System.Collections;
using Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Fish
{
    public class Pettable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private FishAPI fishApi;
        private FishController fishController;
        private CursorManager _cursorManager;
        private FishEmotions fishEmotions;
        private FishDeath fishDeath;

        private int holdCounter = 0;
        private int minutesToGetLonely = 144;
        private Coroutine holdCounterCoroutine;

        private void Start()
        {
            fishApi = GameObject.Find("FishApi").GetComponent<FishAPI>();
            fishController = GetComponent<FishController>();
            fishEmotions = GetComponent<FishEmotions>();
            fishDeath = GetComponent<FishDeath>();
            _cursorManager = FindObjectOfType<CursorManager>();
        
            if (_cursorManager == null)
            {
                Debug.LogError("PettingManager not found in the scene.");
            }
            StartCoroutine(GetLonelyOverTime());
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_cursorManager != null)
            {
                _cursorManager.SetActiveCursorType(CursorManager.CursorType.Petting);
                holdCounterCoroutine = StartCoroutine(CountHoldTime());
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_cursorManager != null)
            {
                _cursorManager.SetActiveCursorType(CursorManager.CursorType.DefaultHand);

                if (holdCounterCoroutine != null && holdCounter >= 5)
                {
                    fishEmotions.SetEmotion(FishEmotions.Emotion.Loved);
                    StopCoroutine(holdCounterCoroutine);
                    _ = fishApi.UploadFishPet(fishController.fishId, holdCounter);
                    fishController.SetSocialLevel(fishController.socialLevel + holdCounter);
                    holdCounter = 0;
                }
            }
        }
        
        private IEnumerator GetLonelyOverTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(minutesToGetLonely * 60f);
                fishController.SetSocialLevel(fishController.socialLevel - 1);
                CheckIfDead();
            }
        }

        private void CheckIfDead()
        {
            if (fishController.socialLevel == 0)
            {
                fishDeath.Die();
            }
        }


        private IEnumerator CountHoldTime()
        {
            holdCounter = 0;
            while (true)
            {
                yield return new WaitForSeconds(1);
                holdCounter+=5;
            }
        }
    }
}