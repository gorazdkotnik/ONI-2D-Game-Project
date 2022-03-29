using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    PlayerController playerController;

    Rigidbody2D rb2d;
    Animator animator;

    [Header("HUD")]
    [SerializeField] GameObject qBar;
    [SerializeField] GameObject rBar;

    [Header("Fire Positions")]
    [SerializeField] Transform firePoint;
    [SerializeField] Transform crouchFirePoint;
    [SerializeField] GameObject specialAttackPoints;

    [Header("Bullet")]
    [SerializeField] GameObject fireBall;

    [Header("Bullet Physics")]
    [SerializeField] float bulletForce = 20f;
    [SerializeField] float fireRate = 0.5f;
    float lastShootTime = 0f;

    [Header("Special Attack")]
    [SerializeField] float specialAttackForce;
    [SerializeField] float specialAttackRate = 10f;
    float lastSpecialAttack = 0f;

    [Header("Effects")]
    [SerializeField] GameObject ultimateEffect;

    [HideInInspector] public bool isSpecialAttacking = false;
    bool isAttacking = false;

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
        UpdateBars();
    }

    void AttackController()
    {
        if (!((playerController.IsGrounded() && !playerController.isMoving) || (playerController.isMoving && playerController.IsGrounded() && playerController.isCrouching) && !isSpecialAttacking))
        {
            return;
        }       

        if (Input.GetKeyDown(KeyCode.Q) && Time.time > lastShootTime + fireRate)
        {
            lastShootTime = Time.time;
            isAttacking = true;
            Shoot();
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            isAttacking = false;
        }
        animator.SetBool("isAttacking", isAttacking);
    }

    void SpecialAttackController()
    {
        if (!isSpecialAttacking && !isAttacking && Input.GetKey(KeyCode.R) && Time.time > lastSpecialAttack + specialAttackRate)
        {
            isSpecialAttacking = true;
            animator.SetBool("isJumping", isSpecialAttacking);

            lastSpecialAttack = Time.time;
            lastShootTime = Time.time;

            rb2d.AddForce(new Vector2(0f, specialAttackForce), ForceMode2D.Impulse);

            Invoke("SpawnFireBalls", 0.15f);

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
        InstantiateUltimateEffect();
    }

    void InstantiateUltimateEffect()
    {
        GameObject effect = Instantiate(ultimateEffect, transform.position, ultimateEffect.transform.rotation);
        Destroy(effect, 0.3f);
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

    void UpdateBars()
    {
        float attackFillAmount = (Time.time - lastShootTime) / fireRate;
        float specialAttackFillAmount = (Time.time - lastSpecialAttack) / specialAttackRate;

        qBar.GetComponent<Image>().fillAmount = attackFillAmount;
        rBar.GetComponent<Image>().fillAmount = specialAttackFillAmount;
    }
}
