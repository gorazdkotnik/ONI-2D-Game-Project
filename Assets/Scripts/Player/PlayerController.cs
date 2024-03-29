using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerAttack playerAttack;

    Rigidbody2D rb2d;
    Animator animator;

    [Header("Layer Masks")]
    [SerializeField] LayerMask groundLayer;

    [Header("Jumping")]
    [SerializeField] GameObject jumpEffectPoint;
    [SerializeField] GameObject jumpEffect;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpRate = 1f;
    float lastJumped = 0f;

    [Header("Moving")]
    [SerializeField] float moveSpeed;
    [SerializeField] float crouchSpeed;
    float moveHorizontal = 0f;

    [Header("Effects")]
    [SerializeField] float effectRate = 0.5f;
    float lastEffect = 0f;

    [HideInInspector] public bool isCrouching = false;
    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public bool facingRight = true;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        playerAttack = GetComponent<PlayerAttack>();

        moveHorizontal = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(moveHorizontal));

        CrouchController();
    }

    void FixedUpdate()
    {
        MoveController();
        JumpController();
    }

    void MoveController()
    {
        if (isSpecialAttacking()) return;

        if (moveHorizontal > 0.1f || moveHorizontal < -0.1f)
        {
            StopAttacking();

            isMoving = true;
            facingRight = moveHorizontal > 0f;

            SetSpriteDirection();
            InstantiateWindEffect();

            float speed = isCrouching ? crouchSpeed : moveSpeed;
            rb2d.AddForce(new Vector2(moveHorizontal * speed * Time.deltaTime, 0f), ForceMode2D.Impulse);

        } else
        {
            isMoving = false;
        }

        if (isMoving && IsGrounded() && !isCrouching)
        {
            if (!FindObjectOfType<AudioManager>().isPlaying("Run")) FindObjectOfType<AudioManager>().Play("Run");
        } else 
        {
            FindObjectOfType<AudioManager>().Stop("Run");
        }
    }

    void JumpController()
    {
        if (isSpecialAttacking()) return;

        bool isJumping = !IsGrounded();
        animator.SetBool("isJumping", isJumping);

        if (!isJumping && Input.GetButton("Jump") && Time.time > lastJumped + jumpRate)
        {
            lastJumped = Time.time;

            InstantiateWindEffect(isJumping);

            rb2d.AddForce(new Vector2(0f, jumpForce * Time.deltaTime), ForceMode2D.Impulse);
            FindObjectOfType<AudioManager>().Play("Jump");
        }
    }

    void CrouchController()
    {
        if (isSpecialAttacking()) return;

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

    void InstantiateWindEffect(bool isJumping = false)
    {
        if ((IsGrounded() && !isCrouching && Time.time > lastEffect + effectRate) || (isJumping))
        {
            Vector3 jumpEffectScale = transform.localScale;
            if (moveHorizontal < -0.1f)
            {
                jumpEffectScale.x = -1;
            } else
            {
                jumpEffectScale.x = 1;
            }
            jumpEffect.transform.localScale = jumpEffectScale;

            lastEffect = Time.time;
            GameObject effect = Instantiate(jumpEffect, jumpEffectPoint.transform.position, jumpEffectPoint.transform.rotation);
            Destroy(effect, 0.2f);
        }
    }

    bool isSpecialAttacking()
    {
        return playerAttack && playerAttack.isSpecialAttacking;
    }

    void StopAttacking() {
        playerAttack.isAttacking = false;
        animator.SetBool("isAttacking", false);
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
