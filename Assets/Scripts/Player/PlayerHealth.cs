using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
  Rigidbody2D rb2d;
  PlayerController playerController;

  [Header("HUD")]
  [SerializeField] GameObject healthBar;
  [SerializeField] GameObject armorBar;

  [Header("UI")]
  [SerializeField] GameObject respawnScreen;
  [SerializeField] TextMeshProUGUI respawnTitleText;

  [Header("Options")]
  [SerializeField] float maxHealth = 100f;
  [SerializeField] float maxArmor = 100f;
  [SerializeField] LayerMask invisibleWall;
  [SerializeField] float enemyDamageRate = 0.5f;
  [SerializeField] float invisibleWallDamage = 10f;
  [SerializeField] float invisibleWallDamageRate = 1f;

  float lastEnemyTouch = 0f;
  float lastInvisibleWallTouch = 0f;

  float currentHealth;
  float currentArmor;

  float survivalTime = 0f;

  [SerializeField] float collisionBounceY = 10f;
  [SerializeField] float collisionBounceX = 50f;

  [Header("Effects")]
  [SerializeField] GameObject hitEffect;

  void Start()
  {
    rb2d = GetComponent<Rigidbody2D>();
    playerController = GetComponent<PlayerController>();

    currentHealth = maxHealth;
    currentArmor = maxArmor;
  }

  void Update()
  {
    UpdateBars();

    PlayerCollisionHandler();

    OnTouchingInvisibleWall();

    survivalTime += Time.deltaTime;
  }

  void UpdateBars()
  {
    healthBar.GetComponent<Image>().fillAmount = currentHealth / maxHealth;
    armorBar.GetComponent<Image>().fillAmount = currentArmor / maxArmor;
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    switch (collision.gameObject.tag.ToLower())
    {
      case "enemy":
        BouncePlayerBack(collision);
        break;
      case "instant kill":
        InstantKill();
        break;
    }

    CheckPlayerDeath();
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    switch (other.gameObject.tag.ToLower())
    {
      case "instant kill":
        InstantKill();
        break;
    }
  }

  void OnTouchingInvisibleWall()
  {
    if (GetComponent<CapsuleCollider2D>().IsTouchingLayers(LayerMask.GetMask("Invisible Wall")))
    {
      if (Time.time - lastInvisibleWallTouch > invisibleWallDamageRate)
      {
        FindObjectOfType<AudioManager>().Play("PlayerHit");
        lastInvisibleWallTouch = Time.time;
        TakeDamage(invisibleWallDamage);
      }
    }
  }

  void CheckPlayerDeath()
  {
    if (currentHealth <= 0)
    {
      FindObjectOfType<AudioManager>().Play("PlayerDeath");
      EndAllSounds();

      Time.timeScale = 0f;
      respawnScreen.SetActive(true);

      respawnTitleText.text = "You died!\nSurvival time: " + survivalTime.ToString("0.00") + " s";
    }
  }

  void EndAllSounds()
  {
    FindObjectOfType<AudioManager>().Stop("Run");
  }

  void PlayerCollisionHandler()
  {
    bool isTouchingEnemies = GetComponent<CapsuleCollider2D>().IsTouchingLayers(LayerMask.GetMask("Enemies"));
    bool isTouchingBosses = GetComponent<CapsuleCollider2D>().IsTouchingLayers(LayerMask.GetMask("Bosses"));

    if (GetComponent<CapsuleCollider2D>().IsTouchingLayers(LayerMask.GetMask("Enemies")) || GetComponent<CapsuleCollider2D>().IsTouchingLayers(LayerMask.GetMask("Bosses")))
    {
      if (Time.time - lastEnemyTouch > enemyDamageRate)
      {
        lastEnemyTouch = Time.time;

        float damage = Random.Range(10, 30);
        if (isTouchingBosses)
        {
          damage = Random.Range(30, 50);
        }
        TakeDamage(damage);

        PlayHitEffect();

        FindObjectOfType<AudioManager>().Play("PlayerHit");

        CheckPlayerDeath();
      }
    };
  }


  void TakeDamage(float damage)
  {
    if (currentArmor > 0f)
    {
      if (currentArmor - damage >= 0f)
      {
        currentArmor -= damage;
      }
      else
      {
        currentHealth -= Mathf.Abs(currentArmor - damage);
        currentArmor = 0f;
      }
    }
    else
    {
      currentHealth -= damage;
    }
  }

  void InstantKill()
  {
    currentArmor = 0f;
    currentHealth = 0f;
  }

  void BouncePlayerBack(Collision2D collision)
  {
    Vector2 bounceDirection = collision.contacts[0].normal;
    bounceDirection.x = bounceDirection.x * collisionBounceX;
    bounceDirection.y = bounceDirection.y * collisionBounceY;

    rb2d.velocity = bounceDirection;
  }

  public bool IsHealthFull()
  {
    return currentHealth == maxHealth;
  }

  public bool IsArmorFull()
  {
    return currentArmor == maxArmor;
  }

  public void UpdateHealth(float amount)
  {
    if (currentHealth + amount <= maxHealth)
    {
      currentHealth += amount;
    }
    else
    {
      currentHealth = maxHealth;
    }
  }

  public void UpdateArmor(float amount)
  {
    if (currentArmor + amount <= maxArmor)
    {
      currentArmor += amount;
    }
    else
    {
      currentArmor = maxArmor;
    }
  }

  void PlayHitEffect()
  {
    GameObject effect = Instantiate(hitEffect, transform.position + new Vector3(0f, 0f, 0f), Quaternion.identity);
    Destroy(effect, 1f);
  }

}
