using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Move
    [SerializeField] float horizontalSpeed = 10f;
    float horizontalAmount;

    // Jump
    [SerializeField] float jumpForce = 20;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float gravityScale = 5;
    bool isGrounded = false;
    float distanceToCheck = 0.5f;
    float velocity;

    void Update()
    {
        // Move left / right
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput > 0.1f || horizontalInput < -0.1f)
        {
            horizontalAmount = horizontalInput * Time.deltaTime * horizontalSpeed;
        } else
        {
            horizontalAmount = 0;
        }

        // Jump
        velocity += gravity * gravityScale * Time.deltaTime;
        if (isGrounded && velocity < 0)
        {
            velocity = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            velocity = jumpForce;
        }

        transform.Translate(horizontalAmount, velocity, 0);

        // Ground check
        if (Physics2D.Raycast(transform.position, Vector2.down, distanceToCheck))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
