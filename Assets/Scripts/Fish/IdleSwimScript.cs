using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class IdleSwimScript : MonoBehaviour
{
    public float speed = 2f;
    public float swayAmplitude = 0.5f;
    public float swayFrequency = 1f;
    public float margin = 10f;
    private Vector3 direction;
    private float originalY;
    private Camera camera;
    private FishFlip fishFlip;
    private float swayTime;


    void Start()
    {
        fishFlip = GetComponent<FishFlip>();
        // Initialize the fish's horizontal direction (randomly left or right)
        direction = Random.Range(0, 2) == 0 ? Vector3.left : Vector3.right;
        originalY = transform.position.y;
        camera = Camera.main;
        FlipFishToFaceDirection();
    }

    private void OnEnable()
    {
        originalY = transform.position.y;
        swayTime = 0f;
        FlipFishToFaceDirection();
    }

    void Update()
    {
        transform.Translate(direction * (speed * Time.deltaTime));
        
        swayTime += Time.deltaTime;

        // Add swaying motion on the Y-axis using a sine wave
        float swayOffset = Mathf.Sin(swayTime * swayFrequency) * swayAmplitude;
        transform.position = new Vector3(transform.position.x, originalY + swayOffset, transform.position.z);

        KeepWithinScreenBounds();
    }

    void KeepWithinScreenBounds()
    {
        Vector3 screenPosition = camera.WorldToScreenPoint(transform.position);

        if (screenPosition.x <= margin || screenPosition.x >= Screen.width - margin)
        {
            direction = -direction;
            FlipFishToFaceDirection();
        }
    }

    void FlipFishToFaceDirection()
    {
        fishFlip.FaceDirection(direction);
    }
}
