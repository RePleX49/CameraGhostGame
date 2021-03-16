using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;

    public float speed = 12f;

    float velocityY;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

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
        Vector3 velocity = transform.right * x + transform.forward * z;
        velocity.Normalize();
        velocity *= speed;

        velocityY += Time.deltaTime * gravity; // apply gravity to velocityY float
        velocity += Vector3.up * velocityY; // add velocityY to final velocity vector

        controller.Move(velocity * Time.deltaTime);
    }
}
