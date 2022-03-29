using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    Rigidbody2D rb2d;
    PlayerController playerController;

    [Header("HUD")]
    [SerializeField] GameObject healthBar;

    [Header("UI")]
    [SerializeField] GameObject respawnScreen;

    [Header("Options")]
    [SerializeField] float maxHealth = 100f;
    float currentHealth;

    [SerializeField] float collisionBounceY = 10f;
    [SerializeField] float collisionBounceX = 50f;

    [Header("Effects")]
    [SerializeField] GameObject hitEffect;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();

        currentHealth = 100f;
    }

    void Update()
    {
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        healthBar.GetComponent<Image>().fillAmount = currentHealth / maxHealth;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag.ToLower())
        {
            case "crab":
                PlayerCollisionHandler(collision, 10f);
                break;
        }

        CheckPlayerDeath();
    }

    void CheckPlayerDeath()
    {
        if (currentHealth <= 0)
        {
            Time.timeScale = 0f;
            respawnScreen.SetActive(true);
        }
    }


    void PlayerCollisionHandler(Collision2D collision, float damage)
    {
        currentHealth -= damage;
        BouncePlayerBack();
        PlayHitEffect();
    }

    void BouncePlayerBack()
    {
        rb2d.AddForce(new Vector2((playerController.facingRight ? -1 : 1) * collisionBounceX, collisionBounceY), ForceMode2D.Impulse);
    }

    void PlayHitEffect()
    {
        GameObject effect = Instantiate(hitEffect, transform.position + new Vector3(0f, 0f, 0f), Quaternion.identity);
        Destroy(effect, 1f);
    }

}
