using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;

    Rigidbody2D rb2d;
    Animator animator;

    [SerializeField] float moveSpeed;
    [SerializeField] float crouchSpeed;
    [SerializeField] float jumpForce;

    float moveHorizontal = 0f;

    [HideInInspector] public bool isCrouching = false;
    [HideInInspector] public bool isMoving = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(moveHorizontal));

        CrouchControler();
    }

    void FixedUpdate()
    {
        MoveController();
        JumpController();
    }

    void MoveController()
    {
        if (moveHorizontal > 0.1f || moveHorizontal < -0.1f)
        {
            isMoving = true;
            SetSpriteDirection();

            float speed = isCrouching ? crouchSpeed : moveSpeed;
            rb2d.AddForce(new Vector2(moveHorizontal * speed, 0f), ForceMode2D.Impulse);

        } else
        {
            isMoving = false;
        }
    }

    void JumpController()
    {
        bool isJumping = !IsGrounded();
        animator.SetBool("isJumping", isJumping);

        if (!isJumping && Input.GetButton("Jump"))
        {
            rb2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    void CrouchControler()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouching = true;
        } 
        else if (Input.GetButtonUp("Crouch"))
        {
            isCrouching = false;
        }
        animator.SetBool("isCrouching", isCrouching);
    }

    void SetSpriteDirection()
    {
        Vector3 characterScale = transform.localScale;
        if (moveHorizontal < -0.1f)
        {
            characterScale.x = -1;
        } else if (moveHorizontal > 0.1f)
        {
            characterScale.x = 1;
        }
        transform.localScale = characterScale;
    }

    public bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.2f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);

        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }
}
