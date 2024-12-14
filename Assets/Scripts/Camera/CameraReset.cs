using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReset : MonoBehaviour
{
    public Vector3 targetPosition;
    public float moveTime = 1f; 

    public void Start()
    {
        targetPosition = transform.position;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ResetCamera();
        }
    }

    public void ResetCamera()
    {
        StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget()
    {
        Vector3 startPosition = transform.position;
        float startTime = Time.time;

        while (Time.time - startTime < moveTime)
        {
            float fractionOfJourney = (Time.time - startTime) / moveTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
            yield return null;
        }

        transform.position = targetPosition;
    }
}