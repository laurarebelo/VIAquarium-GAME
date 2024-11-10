using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleSwim : MonoBehaviour
{
    public float minSpeed = 0.6f;
    public float maxSpeed = 1.5f;
    public float stopDurationMin = 0.5f;
    public float stopDurationMax = 2f;
    private Transform topLeftBoundary;
    private Transform bottomRightBoundary;

    private Vector3 targetPosition;
    private float speed;
    private FishFlip fishFlip;
    private float swayTime;
    private bool isMoving = true;

    private void Awake()
    {
        fishFlip = GetComponent<FishFlip>();
        topLeftBoundary = GameObject.Find("MinBounds").transform;
        bottomRightBoundary = GameObject.Find("MaxBounds").transform;
        targetPosition = transform.position;
        SetNewTargetPosition();
    }

    void Update()
    {
        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * (speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            StartCoroutine(StopAndChooseNewTarget());
        }

        fishFlip.FaceDirection(direction);
    }

    IEnumerator StopAndChooseNewTarget()
    {
        isMoving = false;
        yield return new WaitForSeconds(Random.Range(stopDurationMin, stopDurationMax));
        SetNewTargetPosition();
        isMoving = true;
    }

    void SetNewTargetPosition()
    {
        if (topLeftBoundary != null && bottomRightBoundary != null)
        {
            float minX = topLeftBoundary.position.x;
            float maxX = bottomRightBoundary.position.x;
            float minY = bottomRightBoundary.position.y;
            float maxY = topLeftBoundary.position.y;

            float targetX = Random.Range(minX, maxX);

            float targetY;
            float prevY = targetPosition.y;
            float minLimitedY = Mathf.Max(prevY - 1f, minY);
            float maxLimitedY = Mathf.Min(prevY + 1f, maxY);
            targetY = Random.Range(minLimitedY, maxLimitedY);

            targetPosition = new Vector3(targetX, targetY, transform.position.z);
            speed = Random.Range(minSpeed, maxSpeed);
        }
    }
}
