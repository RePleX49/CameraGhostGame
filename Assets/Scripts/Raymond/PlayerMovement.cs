using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;

    public float walkSpeed = 12f;
    public float velocitySmoothTime = 0.2f;
    Vector3 velocity;
    Vector3 currentVelocity;

    float velocityY;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float airControl = 0.5f;

    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocityY < 0)
        {
            velocityY = -2f;
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // make velocity vector
        Vector3 targetVelocity = transform.right * x + transform.forward * z;
        targetVelocity.Normalize();
        targetVelocity *= walkSpeed;
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
}
