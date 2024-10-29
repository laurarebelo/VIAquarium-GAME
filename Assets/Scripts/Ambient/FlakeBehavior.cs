using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlakeBehavior : MonoBehaviour
{
    private float lifetime;
    private float speed;
    private float size;
    private Gradient colorGradient;

    private Vector3 moveDirection; // New: random movement direction
    private float lifetimeCounter;
    private SpriteRenderer particleSpriteRenderer;
    public int fishClaimed;


    public void Initialize(float lifetime, float speed, float size, Gradient color, Sprite sprite)
    {
        this.lifetime = lifetime;
        this.speed = speed;
        this.size = size;
        this.colorGradient = color;
        fishClaimed = -1;

        lifetimeCounter = 0f;

        transform.localScale = Vector3.one * size;

        GetComponent<Renderer>().material.color = colorGradient.Evaluate(Random.value);
        GetComponent<SpriteRenderer>().sprite = sprite;

        moveDirection = Random.onUnitSphere;
    }

    void Update()
    {
        lifetimeCounter += Time.deltaTime;

        transform.Translate(moveDirection * (speed * Time.deltaTime));

        if (lifetimeCounter >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    public bool Claim(int fishId)
    {
        if (fishClaimed == -1)
        {
            fishClaimed = fishId;
            return true;
        }

        if (fishClaimed == fishId)
        {
            return true;
        }

        return false;
    }
}