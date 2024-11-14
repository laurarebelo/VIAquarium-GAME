using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    private float minSpeed = 0.6f;
    private float maxSpeed = 2f;
    private float stopDurationMin = 0.5f;
    private float stopDurationMax = 2f;
    public float minEscapeDistance = 3f;
    public float maxEscapeDistance = 7f;
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
        SetNewRandomTargetPosition();
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
        SetNewRandomTargetPosition();
        isMoving = true;
    }

    public void SwimAway(Vector3 knockPosition)
    {
        isMoving = false;
        Vector3 fishPosition = transform.position;
        Vector2 fishPosition2D = new Vector2(fishPosition.x, fishPosition.y);
        Vector2 knockPosition2D = new Vector2(knockPosition.x, knockPosition.y);
        Vector2 directionToKnock = knockPosition2D - fishPosition2D;
        Vector2 escapeDirection = -directionToKnock.normalized;
        float randomDistance = Random.Range(minEscapeDistance, maxEscapeDistance);
        Vector2 newTargetPosition2D = fishPosition2D + escapeDirection * randomDistance;
        Vector2 clampedTargetPosition2D = NormalizeVectorWithScreenBounds(newTargetPosition2D);
        targetPosition = new Vector3(clampedTargetPosition2D.x, clampedTargetPosition2D.y, fishPosition.z);
        speed = Random.Range(3, 7);
        isMoving = true;
    }

    private Vector3 NormalizeVectorWithScreenBounds(Vector3 vector)
    {
        Vector3 vectorToReturn = vector;
        float minX = topLeftBoundary.position.x;
        float maxX = bottomRightBoundary.position.x;
        float minY = bottomRightBoundary.position.y;
        float maxY = topLeftBoundary.position.y;

        if (vector.x < minX)
        {
            vectorToReturn.x = minX;
        }

        if (vector.x > maxX)
        {
            vectorToReturn.x = maxX;
        }

        if (vector.y < minY)
        {
            vectorToReturn.y = minY;
        }
        else if (vector.y > maxY)
        {
            vectorToReturn.y = maxY;
        }

        return vectorToReturn;
    }

    void SetNewRandomTargetPosition()
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