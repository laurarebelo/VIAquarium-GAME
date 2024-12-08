using System;
using UnityEngine;

public class CameraArrowMovement : MonoBehaviour
{
    private CameraBounds cameraBounds;
    public float speed = 5f;

    private void Start()
    {
        cameraBounds = GetComponent<CameraBounds>();
    }

    void Update()
    {
        float horizontal = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontal = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontal = 1f;
        }

        Vector3 movement = new Vector3(horizontal, 0, 0) * (speed * Time.deltaTime);

        transform.position += movement;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, cameraBounds.MinX, cameraBounds.MaxX),
            transform.position.y,
            transform.position.z
        );
    }
}