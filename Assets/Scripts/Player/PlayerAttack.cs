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
    [SerializeField] GameObject manaBar;
    [SerializeField] Color baseBarColor = Color.white;
    [SerializeField] Color cooldownBarColor = Color.gray;

    [Header("Mana")]
    [SerializeField] float manaRegenRate = 0.1f;
    [SerializeField] float maxMana = 200f;
    [SerializeField] float qManaCost = 10f;
    [SerializeField] float eManaCost = 20f;
    [SerializeField] float rManaCost = 40f;
    float currentMana;

    [Header("Fire Positions")]
    [SerializeField] Transform firePoint;
    [SerializeField] Transform crouchFirePoint;
    [SerializeField] GameObject specialAttackPoints;

    [Header("Bullet")]
    [SerializeField] GameObject fireBall;
    [SerializeField] GameObject bigFireBall;

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
    [HideInInspector] public bool isAttacking = false;

    float[] specialAttackFireBallRotationsFR = new float[5] {-90f, -60f, -120f, 0f, 180f};
    float[] specialAttackFireBallRotationsFL = new float[5] { -90f, -120f, -60f, 180f, 0f };

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();

        qLastShoot = Time.time;
        eLastShoot = Time.time;
        rLastShoot = Time.time;

        currentMana = maxMana;
    }

    void Update()
    {

        AttackController();
        SpecialAttackController();

        UpdateBars();
        RegenerateMana();

        ShowDoableActions();
    }

    void AttackController()
    {
        if (!AbleToAttack())
        {
            return;
        }       

        if (Input.GetKeyDown(KeyCode.J) && Time.time > qLastShoot + qAttackRate && EnoughMana(qManaCost))
        {
            qLastShoot = Time.time;
            isAttacking = true;

            UpdateMana(-qManaCost);

            Shoot();
        } else if (Input.GetKeyDown(KeyCode.K) && Time.time > eLastShoot + eAttackRate && EnoughMana(eManaCost))
        {
            qLastShoot = Time.time;
            eLastShoot = Time.time;

            UpdateMana(-eManaCost);

            isAttacking = true;
            Shoot();
            Invoke("Shoot", 0.2f);
        }
        else if (Input.GetKeyUp(KeyCode.J) || Input.GetKeyUp(KeyCode.K))
        {
            isAttacking = false;
        }
        animator.SetBool("isAttacking", isAttacking);
    }

    void SpecialAttackController()
    {
        if (AbleToSpecialAttack() && EnoughMana(rManaCost) && Input.GetKey(KeyCode.L) && Time.time > rLastShoot + rAttackRate)
        {
            isSpecialAttacking = true;
            animator.SetBool("isJumping", isSpecialAttacking);

            rLastShoot = Time.time;
            qLastShoot = Time.time;
            eLastShoot = Time.time;

            UpdateMana(-rManaCost);

            rb2d.AddForce(new Vector2(0f, specialAttackForce), ForceMode2D.Impulse);

            Invoke("SpawnFireBalls", 0.15f);

            Invoke("SpawnFireBalls", 0.35f);

            Invoke("EndSpecialAttack", 1f);
        }
    }

    void SpawnFireBalls()
    {
        for (int i = 0; i < specialAttackPoints.transform.childCount; i++)
        {
            Transform firePoint = specialAttackPoints.transform.GetChild(i);

            float zRotation = playerController.facingRight ? specialAttackFireBallRotationsFR[i] : specialAttackFireBallRotationsFL[i];

            InstantiateFireBall(bigFireBall, firePoint, new Vector3(0f, 0f, zRotation));
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
        FindObjectOfType<AudioManager>().Play("FireCracker");
    }

    void UpdateBars()
    {
        float qFillAmount = (Time.time - qLastShoot) / qAttackRate;
        float eFillAmount = (Time.time - eLastShoot) / eAttackRate;
        float rFillAmount = (Time.time - rLastShoot) / rAttackRate;
        float manaFillAmount = currentMana / maxMana;

        qBar.GetComponent<Image>().fillAmount = qFillAmount;
        eBar.GetComponent<Image>().fillAmount = eFillAmount;
        rBar.GetComponent<Image>().fillAmount = rFillAmount;
        manaBar.GetComponent<Image>().fillAmount = manaFillAmount;
    }

    bool AbleToAttack()
    {
        return ((playerController.IsGrounded() && !playerController.isMoving) || (playerController.isMoving && playerController.IsGrounded() && playerController.isCrouching) && !isSpecialAttacking);
    }

    bool AbleToSpecialAttack()
    {
        return !isSpecialAttacking && !isAttacking;
    }

    bool EnoughMana(float manaCost) {
        return currentMana >= manaCost;
    }

    public bool IsManaFull()
    {
        return currentMana >= maxMana;
    }

    public void UpdateMana(float manaCost)
    {
        if (currentMana + manaCost > maxMana)
        {
            currentMana = maxMana;
        }
        else
        {
            currentMana += manaCost;
        }
    }

    // regenerate mana every few seconds
    void RegenerateMana()
    {
        if (currentMana < maxMana)
        {
            currentMana += Time.deltaTime * manaRegenRate;
        }
    }

    void ShowDoableActions()
    {
        if(!AbleToAttack() || !EnoughMana(qManaCost))
        {
            qBar.GetComponent<Image>().color = cooldownBarColor;
        } else {
            qBar.GetComponent<Image>().color = baseBarColor;
        }

        if(!AbleToAttack() || !EnoughMana(eManaCost))
        {
            eBar.GetComponent<Image>().color = cooldownBarColor;
        } else {
            eBar.GetComponent<Image>().color = baseBarColor;
        }
        

        if (!AbleToSpecialAttack() || !EnoughMana(rManaCost))
        {
            rBar.GetComponent<Image>().color = cooldownBarColor;
        }
        else
        {
            rBar.GetComponent<Image>().color = baseBarColor;
        }
    }
}
