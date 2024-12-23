using System.Collections.Generic;
using UnityEngine;

public class FlakeParticleSystem : MonoBehaviour
{
    [Header("Possible Sprites")]
    public List<Sprite> sprites;

    [Header("Base Settings")] 
    public float duration = 0.25f;
    public float startDelay = 0f;
    public float startLifetime = 20f;
    public float startSpeed = 0.1f;
    public float startSize = 0.4f;
    public Gradient startColor;

    [Header("Emission")]
    public int emissionRate = 20;
    public float emissionInterval = 0.02f;
    private float emissionTimer = 0f;


    [Header("Shape")]
    public float radius = 0.3f;

    public GameObject particlePrefab;

    private float elapsedTime = 0f;

    void Start()
    {
        startColor = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[3];
        colorKeys[0] = new GradientColorKey(Color.green, 0.0f);
        colorKeys[1] = new GradientColorKey(Color.yellow, 0.5f);
        colorKeys[2] = new GradientColorKey(Color.red, 1.0f);
        startColor.colorKeys = colorKeys;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        emissionTimer += Time.deltaTime;

        if (emissionTimer >= emissionInterval)
        {
            InstantiateParticle();
            emissionTimer = 0f;
        }

        if (elapsedTime >= startDelay && elapsedTime >= duration)
        {
            Destroy(gameObject);
        }
    }

    private void InstantiateParticle()
    {
        Vector3 randomPosition = transform.position + Random.onUnitSphere * radius;
        GameObject particle = Instantiate(particlePrefab, randomPosition, Quaternion.identity);
        FlakeBehavior behavior = particle.AddComponent<FlakeBehavior>();
        Sprite sprite = Utils.GetRandomItem(sprites);
        behavior.Initialize(startLifetime, startSpeed, startSize, startColor, sprite);
    }
}