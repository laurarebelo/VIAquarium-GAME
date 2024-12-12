using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraDragMovement : MonoBehaviour
{
    private Vector2 initialDragPosition;
    private bool isDragging = false;

    private Vector3 velocity;
    private Vector3 maxVelocity;
    public float maxVelocityDuration = 0.2f;
    private float velocityTimer = 0f;
    public float dragMultiplier = 0.0075f;
    public float velocityMultiplier = 0.005f;
    public float deceleration = 0.95f;
    
    private Vector3 initialCameraPosition;
    private Vector3 lastPosition;

    private CameraBounds cameraBounds;
    private HandState handState;

    private void Start()
    {
        cameraBounds = GetComponent<CameraBounds>();
        lastPosition = transform.position;
        GameObject handStateGo = GameObject.Find("HandState");
        if (handStateGo)
        {
            handState = handStateGo.GetComponent<HandState>();
        }
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (handState != null && handState.isPetting) return;
            initialDragPosition = Input.mousePosition;
            initialCameraPosition = transform.position;
            isDragging = true;
            velocityTimer = 0f;
            maxVelocity = Vector3.zero;
        }

        if (isDragging)
        {
            Vector2 currentDragPosition = Input.mousePosition;
            Vector2 dragDelta = initialDragPosition - currentDragPosition;
            

            transform.position = initialCameraPosition + new Vector3(dragDelta.x, 0, 0) * dragMultiplier;
            
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, cameraBounds.MinX, cameraBounds.MaxX),
                transform.position.y,
                transform.position.z
            );
            
            velocity = (transform.position - lastPosition) / Time.deltaTime * dragMultiplier;
            if (velocity.magnitude > maxVelocity.magnitude)
            {
                maxVelocity = velocity;
            }
            velocityTimer += Time.deltaTime;

        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            velocity = maxVelocity * velocityMultiplier;
            isDragging = false;
        }

        lastPosition = transform.position;

    }

    private void FixedUpdate()
    {
        if (!isDragging && velocity != Vector3.zero)
        {
            transform.position += velocity;
            
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, cameraBounds.MinX, cameraBounds.MaxX),
                transform.position.y,
                transform.position.z
            );
        
            velocity *= deceleration;
            if (velocity.magnitude < 0.001f) 
            {
                velocity = Vector3.zero;
            }
        }
        if (velocityTimer > maxVelocityDuration)
        {
            maxVelocity = Vector3.zero;
        }
    }
}