using UnityEngine;

public class IdleSwimScript : MonoBehaviour
{
    public float speed = 2f;           // Movement speed along X-axis
    public float margin = 10f;         // Margin from screen edges

    private Vector3 direction;         // Stores the current direction (left or right)

    void Start()
    {
        // Initialize the fish's horizontal direction (randomly left or right)
        direction = Random.Range(0, 2) == 0 ? Vector3.left : Vector3.right;
    }

    void Update()
    {
        // Move the fish horizontally
        transform.Translate(direction * speed * Time.deltaTime);

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

            // Optionally rotate the fish to face the new direction
            Vector3 newScale = transform.localScale;
            newScale.x = direction == Vector3.left ? Mathf.Abs(newScale.x) * -1 : Mathf.Abs(newScale.x);
            transform.localScale = newScale;
        }

        // Clamp Y position to prevent vertical movement (keep the fish at the current Y)
        Vector3 clampedWorldPosition = transform.position;
        clampedWorldPosition.y = Camera.main.ScreenToWorldPoint(new Vector3(0, screenPosition.y, screenPosition.z)).y;
        transform.position = new Vector3(transform.position.x, clampedWorldPosition.y, transform.position.z);
    }
}
