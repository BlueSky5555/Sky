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
    public AudioClip landClip;
    public AudioClip jumpClip;  // เพิ่มเสียงกระโดด
    public float stepInterval = 0.5f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isCrouching;

    private float nextStepTime = 0f;
    private bool wasInAir = false;
    private bool isJumping = false;

    // สถานะเสียงกระโดด
    private bool hasPlayedJumpSound = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;
    }

    void Update()
    {
        bool groundedNow = controller.isGrounded;

        // ตรวจจับการลอย/ลงพื้น
        if (!wasInAir && !groundedNow)
        {
            wasInAir = true;
            isJumping = true;  // เริ่มการกระโดด
        }
        else if (wasInAir && groundedNow)
        {
            // เมื่อแตะพื้นแล้ว
            if (landClip && audioSource)
            {
                audioSource.PlayOneShot(landClip);  // เล่นเสียงตกกระทบ
            }
            wasInAir = false;
            isJumping = false;  // สิ้นสุดการกระโดด
            hasPlayedJumpSound = false;  // รีเซ็ตสถานะเสียงกระโดด
        }

        isGrounded = groundedNow;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // ลดความเร็วในการตก
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Determine speed
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
            wasInAir = true;

            // เล่นเสียงกระโดดเมื่อกระโดด
            if (jumpClip && audioSource && !hasPlayedJumpSound)
            {
                audioSource.PlayOneShot(jumpClip);  // เสียงกระโดด
                hasPlayedJumpSound = true;  // ตั้งค่าสถานะเสียงกระโดด
            }
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
