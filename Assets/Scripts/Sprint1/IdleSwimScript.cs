using UnityEngine;

public class IdleSwimScript : MonoBehaviour
{
    public float speed = 2f;
    public float swayAmplitude = 0.5f;
    public float swayFrequency = 1f;
    public float margin = 10f;
    private Vector3 direction;
    private float originalY;
    private Camera camera;

    void Start()
    {
        // Initialize the fish's horizontal direction (randomly left or right)
        direction = Random.Range(0, 2) == 0 ? Vector3.left : Vector3.right;
        originalY = transform.position.y;
        camera = Camera.main;
        FlipFishToFaceDirection();
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        // Add swaying motion on the Y-axis using a sine wave
        float swayOffset = Mathf.Sin(Time.time * swayFrequency) * swayAmplitude;
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
        Vector3 newScale = transform.localScale;
        newScale.x = direction == Vector3.left ? Mathf.Abs(newScale.x) * -1 : Mathf.Abs(newScale.x);
        transform.localScale = newScale;
    }
}
