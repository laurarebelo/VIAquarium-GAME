using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpeningSceneAnimation : MonoBehaviour
{
    public Image blackScreen;
    public TMP_Text RIP_TMP;
    public Image[] fishImages;
    public TMP_Text line1;
    public TMP_Text line2;
    public TMP_Text line3;
    public TMP_Text line4;

    public Image buttonImage;
    public TMP_Text buttonText;

    private void Start()
    {
        if (RestartGameState.Instance.isFirstTime)
        {
            SetInitialAlpha();
            StartCoroutine(PlayAnimationSequence());
        }
        else
        {
            blackScreen.enabled = false;
        }
    }

    private void SetInitialAlpha()
    {
        SetAlpha(blackScreen, 1f);
        SetAlpha(RIP_TMP, 0f);
        foreach (var fish in fishImages) SetAlpha(fish, 0f);
        SetAlpha(line1, 0f);
        SetAlpha(line2, 0f);
        SetAlpha(line3, 0f);
        SetAlpha(line4, 0f);
        SetAlpha(buttonImage, 0f);
        SetAlpha(buttonText, 0f);
    }

    private void SetAlpha(Graphic graphic, float alpha)
    {
        if (graphic != null)
        {
            Color color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
    }

    private IEnumerator PlayAnimationSequence()
    {
        yield return StartCoroutine(FadeImage(blackScreen, 2f, false));
        StartCoroutine(FadeText(RIP_TMP, 2f));
        foreach (var fish in fishImages)
        {
            if (fish != null) StartCoroutine(FadeImage(fish, 2f, true));
        }

        yield return new WaitForSecondsRealtime(2f);
        yield return StartCoroutine(FadeText(line1, 1.5f));
        yield return StartCoroutine(FadeText(line2, 1.5f));
        yield return StartCoroutine(FadeText(line3, 1.5f));
        yield return StartCoroutine(FadeText(line4, 1.5f));

        StartCoroutine(FadeImage(buttonImage, 1.5f, true));
        yield return StartCoroutine(FadeText(buttonText, 1f));
        blackScreen.enabled = false;
    }

    private IEnumerator FadeImage(Image image, float duration, bool fadeIn)
    {
        if (image == null) yield break;
        Color color = image.color;
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            image.color = color;
            yield return null;
        }

        color.a = endAlpha;
        image.color = color;
    }

    private IEnumerator FadeText(TMP_Text text, float duration, bool fadeIn = true)
    {
        if (text == null) yield break;

        Color color = text.color;
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            text.color = color;
            yield return null;
        }

        color.a = endAlpha; 
        text.color = color;
    }
}
