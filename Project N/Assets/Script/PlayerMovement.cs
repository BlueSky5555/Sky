using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    private float originalHeight;

    [Header("Footstep Settings")]
    public AudioSource audioSource;
    public AudioClip walkClip;
    public AudioClip runClip;
    public AudioClip jumpClip;
    public AudioClip landClip; // เสียงกระโดดลงพื้น
    public float stepInterval = 0.5f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isCrouching;

    private float nextStepTime = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

            // เล่นเสียงกระโดดเมื่อถึงพื้น
            if (landClip && audioSource && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(landClip);
            }
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Determine current speed
        float speed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
            speed = runSpeed;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
            controller.height = crouchHeight;
            speed = crouchSpeed;
        }
        else
        {
            isCrouching = false;
            controller.height = originalHeight;
        }

        controller.Move(move * speed * Time.deltaTime);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // เล่นเสียงกระโดด
            if (jumpClip && audioSource)
                audioSource.PlayOneShot(jumpClip);
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Footstep sound
        if (isGrounded && move.magnitude > 0.1f)
        {
            if (Time.time >= nextStepTime)
            {
                PlayFootstepSound(speed);
                nextStepTime = Time.time + stepInterval;
            }
        }
        else
        {
            // หยุดเสียงเท้าเมื่อไม่ได้เดินหรืออยู่กลางอากาศ
            if (audioSource.isPlaying && (audioSource.clip == walkClip || audioSource.clip == runClip))
            {
                audioSource.Stop();
            }
        }
    }

    void PlayFootstepSound(float speed)
    {
        if (audioSource && (walkClip || runClip))
        {
            AudioClip clipToPlay = (speed == runSpeed) ? runClip : walkClip;

            if (audioSource.clip != clipToPlay)
            {
                audioSource.clip = clipToPlay;
            }

            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }
}