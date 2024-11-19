using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishKnockReaction : MonoBehaviour
{
    private Knocker knocker;
    private FishMovement swimmingController;
    public float distanceToReact;

    void Start()
    {
        knocker = GameObject.Find("Knocker").GetComponent<Knocker>();
        if (knocker != null)
        {
            knocker.OnDoubleTap += ReactToKnock;
        }

        swimmingController = GetComponent<FishMovement>();
    }

    bool WasItCloseEnough(Vector3 position)
    {
        Vector3 fishPosition = transform.position;
        Vector2 fish2Dposition = fishPosition;
        Vector2 knock2Dposition = position;
        float distance = Vector2.Distance(fish2Dposition, knock2Dposition);
        return distance < distanceToReact;
    }

    void ReactToKnock(Vector3 position)
    {
        if (WasItCloseEnough(position))
        {
            swimmingController.SwimAway(position);
        }
    }
}