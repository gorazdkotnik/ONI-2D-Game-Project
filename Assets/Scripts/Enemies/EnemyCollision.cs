using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCollision : MonoBehaviour
{
    AudioManager audioManager;

    [Header("Health Bar")]
    [SerializeField] GameObject healthBar;

    [Header("Effects")]
    [SerializeField] GameObject hitEffect;

    [Header("Items")]
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject gemPrefab;

    [Header("Options")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] int minCoins = 1;
    [SerializeField] int maxCoins = 3;
    [SerializeField] int minGems = 1;
    [SerializeField] int maxGems = 3;
    float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        audioManager = FindObjectOfType<AudioManager>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FireBall")
        {
            audioManager.Play("EnemyHit");

            GameObject effect = Instantiate(hitEffect, transform.position + new Vector3(0f, 0f, 0f), Quaternion.identity);
            Destroy(effect, 1f);

            currentHealth -= 20f;

            UpdateHealthBar();
            CheckEnemyDeath();
        }
    }

    void CheckEnemyDeath()
    {
        if (currentHealth <= 0f)
        {
            InstantiateCoins();
            InstantiateGems();
            Destroy(gameObject);
        }
    }

    void InstantiateCoins() {
        // instantiate number of coins between min and max into random direction
        int coins = Random.Range(minCoins, maxCoins);
        for (int i = 0; i < coins; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            coin.transform.localScale = new Vector3(2f, 2f, 2f);
            RandomizeDirection(coin);

        }
    }

    void InstantiateGems() {
        // instantiate number of gems between min and max into random direction
        int gems = Random.Range(minGems, maxGems);
        for (int i = 0; i < gems; i++)
        {
            GameObject gem = Instantiate(gemPrefab, transform.position, Quaternion.identity);
            gem.transform.localScale = new Vector3(2f, 2f, 2f);
            RandomizeDirection(gem);
        }
    }

    void RandomizeDirection(GameObject prefab) {
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);

        prefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(randomX, randomY) * 1000f);
    }

    void UpdateHealthBar()
    {
        healthBar.transform.localScale = new Vector3(currentHealth / maxHealth, 1f, 1f);
    }
}
