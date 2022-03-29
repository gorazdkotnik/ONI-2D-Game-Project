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

    [Header("Options")]
    float currentHealth;
    [SerializeField] float maxHealth = 100f;

    [SerializeField] float collisionBounceY = 10f;
    [SerializeField] float collisionBounceX = 50f;

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
                CrabCollisionHandler(collision);
                break;
        }
    }


    void CrabCollisionHandler(Collision2D collision)
    {
        currentHealth -= 10f;
        rb2d.AddForce(new Vector2((playerController.facingRight ? -1 : 1) * collisionBounceX, collisionBounceY), ForceMode2D.Impulse);
    }
}
