using UnityEngine;

public class IdleSwimScript : MonoBehaviour
{
    public float speed = 2f;           // Movement speed along X-axis
    public float swayAmplitude = 0.5f; // Amplitude of the up and down sway
    public float swayFrequency = 1f;   // Frequency of the sway (how fast it moves up and down)
    public float margin = 10f;         // Margin from screen edges

    private Vector3 direction;         // Stores the current horizontal direction (left or right)
    private float originalY;           // The original Y position to sway around

    void Start()
    {
        // Initialize the fish's horizontal direction (randomly left or right)
        direction = Random.Range(0, 2) == 0 ? Vector3.left : Vector3.right;

        // Store the initial Y position to use for the swaying movement
        originalY = transform.position.y;

        // Immediately flip the fish to face the correct direction
        FlipFishToFaceDirection();
    }

    void Update()
    {
        // Move the fish horizontally
        transform.Translate(direction * speed * Time.deltaTime);

        // Add swaying motion on the Y-axis using a sine wave
        float swayOffset = Mathf.Sin(Time.time * swayFrequency) * swayAmplitude;
        transform.position = new Vector3(transform.position.x, originalY + swayOffset, transform.position.z);

        // Keep fish within the horizontal screen bounds
        KeepWithinScreenBounds();
    }

    void KeepWithinScreenBounds()
    {
        // Convert the fish position to screen space
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        // Check if the fish hits the screen edge on the X-axis
        if (screenPosition.x <= margin || screenPosition.x >= Screen.width - margin)
        {
            // Change direction (flip horizontally)
            direction = -direction;

            // Flip the fish to face the new direction
            FlipFishToFaceDirection();
        }
    }

    void FlipFishToFaceDirection()
    {
        // Flip the fish based on the direction (left or right)
        Vector3 newScale = transform.localScale;
        newScale.x = direction == Vector3.left ? Mathf.Abs(newScale.x) * -1 : Mathf.Abs(newScale.x);
        transform.localScale = newScale;
    }
}
