using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, vertical, 0) * (speed * Time.deltaTime);

        transform.position += movement;
    }
}