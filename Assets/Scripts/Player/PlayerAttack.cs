using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerController playerController;
    Rigidbody2D rb2d;
    Animator animator;

    [SerializeField] Transform firePoint;
    [SerializeField] Transform crouchFirePoint;
    [SerializeField] GameObject fireBall;

    [SerializeField] GameObject specialAttackPoints;

    [SerializeField] float bulletForce = 20f;
    bool isAttacking = false;

    [SerializeField] float fireRate = 0.5f;
    float lastShootTime = 0f;

    [HideInInspector] public bool isSpecialAttacking = false;

    [SerializeField] float specialAttackForce;
    [SerializeField] float specialAttackRate = 10f;
    float lastSpecialAttack = 0f;

    float[] specialAttackFireBallRotations = new float[5] {-90f, -60f, -120f, 0f, 180f};

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {

        AttackController();
        SpecialAttackController();
    }

    void AttackController()
    {
        if (!((playerController.IsGrounded() && !playerController.isMoving) || (playerController.isMoving && playerController.IsGrounded() && playerController.isCrouching) && !isSpecialAttacking))
        {
            return;
        }       

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

    void SpecialAttackController()
    {
        if (!isSpecialAttacking && Input.GetKey(KeyCode.R) && Time.time > lastSpecialAttack + specialAttackRate)
        {
            isSpecialAttacking = true;
            animator.SetBool("isJumping", isSpecialAttacking);

            lastSpecialAttack = Time.time;
            rb2d.AddForce(new Vector2(0f, specialAttackForce), ForceMode2D.Impulse);

            Invoke("SpawnFireBalls", 0.2f);

            Invoke("EndSpecialAttack", 1f);
        }
    }

    void SpawnFireBalls()
    {
        for (int i = 0; i < specialAttackPoints.transform.childCount; i++)
        {
            Transform firePoint = specialAttackPoints.transform.GetChild(i);
            InstantiateFireBall(fireBall, firePoint, new Vector3(0f, 0f, specialAttackFireBallRotations[i]));
        }
    }

    void EndSpecialAttack()
    {
        isSpecialAttacking = false;
        animator.SetBool("isJumping", isSpecialAttacking);
    }

    void Shoot()
    {
        Vector3 rotation = new Vector3(0f, 0f, 0f);

        if (!playerController.facingRight)
        {
            rotation.z = 180f;
        }

        InstantiateFireBall(fireBall, playerController.isCrouching ? crouchFirePoint : firePoint, rotation);
    }

    void InstantiateFireBall(GameObject fireBall, Transform firePoint, Vector3 rotation)
    {
        GameObject bullet = Instantiate(fireBall, firePoint.position, Quaternion.Euler(rotation));
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}
