using System.Collections;
using UnityEngine;

public class FishDeath : MonoBehaviour
{
    private ClickableTrigger clickableTrigger;
    private FishFlip fishFlip;
    private bool isDead = false;
    private Vector3 originalPosition;
    private Transform topLeftBoundary;
    private Transform bottomRightBoundary;
    private FishController fishController;
    
    public float floatSpeed = 0.5f;
    public float waveAmplitude = 0.5f;
    public float waveFrequency = 1f;

    void Start()
    {
        clickableTrigger = GetComponent<ClickableTrigger>();
        fishFlip = GetComponent<FishFlip>();
        
        originalPosition = transform.position;
        
        topLeftBoundary = GameObject.Find("MinBounds").transform;
        bottomRightBoundary = GameObject.Find("MaxBounds").transform;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void TriggerDeath()
    {
        if (!isDead)
        {
            isDead = true;
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        if (clickableTrigger != null)
        {
            clickableTrigger.enabled = false;
        }
        
        fishFlip.FlipVertically();
        //fishController.GetOutline();
        //fishController.SetFishTemplate();
        
        StartCoroutine(FloatUpInWavyLine());
    }

    private IEnumerator FloatUpInWavyLine()
    {
        float elapsedTime = 0f;


        while (transform.position.y < originalPosition.y + 5f)
        {
            elapsedTime += Time.deltaTime;

            float newX = originalPosition.x + Mathf.Sin(elapsedTime * waveFrequency) * waveAmplitude;
            float newY = originalPosition.y + elapsedTime * floatSpeed;

            newX = Mathf.Clamp(newX, topLeftBoundary.position.x, bottomRightBoundary.position.x);
            newY = Mathf.Clamp(newY, bottomRightBoundary.position.y, topLeftBoundary.position.y);

            transform.position = new Vector3(newX, newY, originalPosition.z);

            yield return null;
        }
    }
}