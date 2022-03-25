using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerController playerController;
    Animator animator;

    [SerializeField] Transform firePoint;
    [SerializeField] GameObject fireBall;

    [SerializeField] float bulletForce = 20f;
    bool isAttacking = false;

    [SerializeField] float fireRate = 0.5f;
    float lastShootTime = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if ((playerController.IsGrounded() && !playerController.isMoving) || (playerController.isMoving && playerController.IsGrounded() && playerController.isCrouching))
        {
            AttackController();
        }
    }

    void AttackController()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > lastShootTime + fireRate)
        {
            lastShootTime = Time.time;
            isAttacking = true;
            Shoot();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isAttacking = false;
        }
        animator.SetBool("isAttacking", isAttacking);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(fireBall, firePoint.position, fireBall.transform.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}
