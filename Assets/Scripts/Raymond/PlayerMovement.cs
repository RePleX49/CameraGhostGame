using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;

    public float walkSpeed = 12f;
    public float sprintSpeed = 15f;
    public float velocitySmoothTime = 0.2f;

    public AudioSource footstepAudio;
    public float delayBetweenSteps = 1.0f;
    Vector3 velocity;
    Vector3 currentVelocity;

    float velocityY;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float airControl = 0.5f;

    bool isGrounded;
    bool movementDisabled;

    bool isWalking;

    float baseFootStepPitch;
    float lastStepTime;

    // Start is called before the first frame update
    void Start()
    {
        GameServices.moveController = this;

        controller = GetComponent<CharacterController>();
        movementDisabled = false;
        isWalking = false;
        baseFootStepPitch = footstepAudio.pitch;
    }

    // Update is called once per frame
    void Update()
    {
        if(movementDisabled)
        {
            CancelInvoke("PlaySteps");
            isWalking = false;
            return;
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            GameServices.playerStats.ConsumePill();
        }

        bool isSprinting = Input.GetAxisRaw("Sprint") > 0;
        isGrounded = controller.isGrounded;

        if (isGrounded && velocityY < 0)
        {
            velocityY = -2f;
        }

        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Move(x, z, isSprinting);
    }

    void Move(float x, float z, bool isSprinting)
    {
        // make velocity vector
        Vector3 targetVelocity = transform.right * x + transform.forward * z;
        targetVelocity.Normalize();

        if (targetVelocity.magnitude > 0 && !isWalking && isGrounded)
        {
            isWalking = true;
            InvokeRepeating("PlaySteps", 0.0f, delayBetweenSteps);
        }
        else if (targetVelocity.magnitude == 0 || !isGrounded)
        {
            CancelInvoke("PlaySteps");
            isWalking = false;
        }

        float moveSpeed = isSprinting ? sprintSpeed : walkSpeed;
        targetVelocity *= moveSpeed;
        if (!isGrounded)
        {
            targetVelocity *= airControl;
        }

        // Using SmoothDamp to interpolate velocity
        velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref currentVelocity, velocitySmoothTime);
        velocityY += Time.deltaTime * gravity; // apply gravity to velocityY float

        Vector3 finalVelocity = velocity + (Vector3.up * velocityY);

        controller.Move(finalVelocity * Time.deltaTime);
    }

    void Jump()
    {
        if (controller.isGrounded)
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }      
    }

    public void DisableMovement()
    {
        movementDisabled = true;
    }

    public void EnableMovement()
    {
        movementDisabled = false;
    }

    void PlaySteps()
    {
        // Check remainder delay from last step to prevent sound spamming
        if((lastStepTime + delayBetweenSteps - Time.time) > 0.1f)
        {
            return;
        }

        lastStepTime = Time.time;
        float randPitch = Random.Range(-0.1f, 0.1f);
        footstepAudio.pitch = baseFootStepPitch + randPitch;
        footstepAudio.Play();
    }
}
