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
    [SerializeField] GameObject eBar;

    [Header("Fire Positions")]
    [SerializeField] Transform firePoint;
    [SerializeField] Transform crouchFirePoint;
    [SerializeField] GameObject specialAttackPoints;

    [Header("Bullet")]
    [SerializeField] GameObject fireBall;

    [Header("Bullet Physics")]
    [SerializeField] float bulletForce = 20f;
    [SerializeField] float qAttackRate = 0.5f;
    float qLastShoot = 0f;

    [Header("Secondary Attack")]
    [SerializeField] float eAttackRate = 4f;
    float eLastShoot = 0f;

    [Header("Special Attack")]
    [SerializeField] float specialAttackForce;
    [SerializeField] float rAttackRate = 10f;
    float rLastShoot = 0f;

    [Header("Effects")]
    [SerializeField] GameObject rEffect;

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
        ShowDoableActions();
    }

    void AttackController()
    {
        if (!AbleToAttack())
        {
            return;
        }       

        if (Input.GetKeyDown(KeyCode.Q) && Time.time > qLastShoot + qAttackRate)
        {
            qLastShoot = Time.time;
            isAttacking = true;
            Shoot();
        } else if (Input.GetKeyDown(KeyCode.E) && Time.time > eLastShoot + eAttackRate)
        {
            qLastShoot = Time.time;
            eLastShoot = Time.time;

            isAttacking = true;
            Shoot();
            Invoke("Shoot", 0.2f);
        }
        else if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
        {
            isAttacking = false;
        }
        animator.SetBool("isAttacking", isAttacking);
    }

    void SpecialAttackController()
    {
        if (AbleToSpecialAttack() && Input.GetKey(KeyCode.R) && Time.time > rLastShoot + rAttackRate)
        {
            isSpecialAttacking = true;
            animator.SetBool("isJumping", isSpecialAttacking);

            rLastShoot = Time.time;
            qLastShoot = Time.time;
            eLastShoot = Time.time;

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
        GameObject effect = Instantiate(rEffect, transform.position, rEffect.transform.rotation);
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
        float qFillAmount = (Time.time - qLastShoot) / qAttackRate;
        float eFillAmount = (Time.time - eLastShoot) / eAttackRate;
        float rFillAmount = (Time.time - rLastShoot) / rAttackRate;

        qBar.GetComponent<Image>().fillAmount = qFillAmount;
        eBar.GetComponent<Image>().fillAmount = eFillAmount;
        rBar.GetComponent<Image>().fillAmount = rFillAmount;
    }

    bool AbleToAttack()
    {
        return ((playerController.IsGrounded() && !playerController.isMoving) || (playerController.isMoving && playerController.IsGrounded() && playerController.isCrouching) && !isSpecialAttacking);
    }

    bool AbleToSpecialAttack()
    {
        return !isSpecialAttacking && !isAttacking;
    }

    void ShowDoableActions()
    {
        if(!AbleToAttack())
        {
            qBar.GetComponent<Image>().color = Color.gray;
            eBar.GetComponent<Image>().color = Color.gray;
        } else
        {
            qBar.GetComponent<Image>().color = Color.white;
            eBar.GetComponent<Image>().color = Color.white;
        }

        if (!AbleToSpecialAttack())
        {
            rBar.GetComponent<Image>().color = Color.gray;
        } else
        {
            rBar.GetComponent<Image>().color = Color.white;
        }
    }
}
