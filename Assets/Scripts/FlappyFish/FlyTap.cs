using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyTap : MonoBehaviour
{
    public float velocity = 1.5f;
    public float rotationSpeed = 10f;
    private Rigidbody2D rigidbody;
    private AudioSource audioPlayer;

    void Start()
    {
        audioPlayer = GameObject.Find("TapAudioPlayer").GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody.velocity = Vector2.up * velocity;
            audioPlayer.Stop();
            audioPlayer.Play();
        }
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0,0, rigidbody.velocity.y * rotationSpeed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _ = GameManager.instance.GameOver();
    }
}
