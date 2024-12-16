using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class FishDeath : MonoBehaviour
{
    private FishController fishController;
    public SpriteRenderer lineFishRenderer;
    public SpriteRenderer colorFishRenderer;
    public TMP_Text fishNameText;
    public float floatSpeed = 1f;
    public float waveAmplitude = 0.5f;
    public float waveFrequency = 2f;
    public float fadeDuration = 2f;
    private Transform topLeftBoundary;
    private FishState fishState;
    private FishStore fishStore;

    void Start()
    {
        fishStore = FishStore.Instance;
        fishState = GetComponent<FishState>();
        fishController = GetComponent<FishController>();
        topLeftBoundary = GameObject.Find("MinBounds").transform;
    }

    public void Die()
    {
        StartCoroutine(DieRoutine());
    }

    private IEnumerator DieRoutine()
    {
        fishState.Die();
        fishController.ChangeToDeadOutline();
        fishStore.RemoveFish(fishController.fishId);
        if (fishStore.HasDeadStoredFish())
        {
            Task<List<DeadFishGetObject>> task = FishAPI.Instance.GetDeadFish("lastdied", fishController.fishName, 0, 1);
            while (!task.IsCompleted)
            {
                yield return null;
            }
            DeadFishGetObject deadFish = task.Result[0];
            fishStore.StoreDeadFish(deadFish);
        }
        lineFishRenderer.flipY = true;
        colorFishRenderer.flipY = true;

        yield return new WaitForSeconds(1f);

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(
            transform.position.x,
            topLeftBoundary.position.y,
            transform.position.z
        );

        float time = 0;
        while (transform.position.y < targetPosition.y)
        {
            time += Time.deltaTime;

            float waveOffset = Mathf.Sin(time * waveFrequency) * waveAmplitude;
            transform.position = new Vector3(
                startPosition.x + waveOffset,
                transform.position.y + (floatSpeed * Time.deltaTime),
                transform.position.z
            );

            yield return null;
        }
        yield return new WaitForSeconds(2f);
        FadeOut();
    }

    void FadeOut()
    {
        Transform[] elementsToFade =
        {
            lineFishRenderer.transform,
            colorFishRenderer.transform,
            fishNameText.transform
        };
        foreach (Transform child in elementsToFade)
        {
            var spriteRenderer = child.GetComponent<SpriteRenderer>();
            var text = child.GetComponent<TextMeshProUGUI>();
            if (spriteRenderer != null)
            {
                StartCoroutine(FadeElementOut(spriteRenderer));
            }

            if (text != null)
            {
                StartCoroutine(FadeElementOut(text));
            }
        }
    }

    private IEnumerator FadeElementOut(Object element)
    {
        Color color = Color.white;

        if (element is SpriteRenderer spriteRenderer)
        {
            color = spriteRenderer.color;
        }
        else if (element is Graphic graphic)
        {
            color = graphic.color;
        }
        else
        {
            yield break; // Unsupported type
        }

        float startAlpha = color.a;
        float targetAlpha = 0;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t / fadeDuration);

            if (element is SpriteRenderer sr)
            {
                sr.color = color;
            }
            else if (element is Graphic g)
            {
                g.color = color;
            }

            yield return null;
        }

        color.a = targetAlpha;

        if (element is SpriteRenderer finalSr)
        {
            finalSr.color = color;
        }
        else if (element is Graphic finalG)
        {
            finalG.color = color;
        }
        Destroy(gameObject);
    }

}