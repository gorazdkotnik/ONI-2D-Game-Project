using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollision : MonoBehaviour
{
    PlayerAttack playerAttack;
    PlayerHealth playerHealth;

    [Header("Effects")]
    [SerializeField] GameObject itemPickupEffect;

    [Header("Options")]
    [SerializeField] float minHealthIncrease = 10f;
    [SerializeField] float maxHealthIncrease = 100f;
    [SerializeField] float minArmorIncrease = 10f;
    [SerializeField] float maxArmorIncrease = 100f;
    [SerializeField] float minManaIncrease = 10f;
    [SerializeField] float maxManaIncrease = 100f;


    void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        string tag = other.gameObject.tag.ToLower();

        switch(tag) {
            case "mana potion":
                HandleManaPotion(other);
                break;
            case "health potion":
                HandleHealthPotion(other);
                break;
            case "armor": 
                HandleArmor(other);
                break;    
        }
    }

    void HandleManaPotion(Collider2D other) {
        // if health is full return
        if (playerAttack.IsManaFull()) {
            return;
        }

        // increase mana betwen min an max value
        float manaIncrease = Random.Range(minManaIncrease, maxManaIncrease);
        playerAttack.UpdateMana(manaIncrease);
        Destroy(other.gameObject);

        PlayEffect(transform.position);
    }

    void HandleHealthPotion(Collider2D other) {
        // if health is full return
        if (playerHealth.IsHealthFull()) {
            return;
        }

        // increase health betwen min an max value
        float healthIncrease = Random.Range(minHealthIncrease, maxHealthIncrease);
        playerHealth.UpdateHealth(healthIncrease);
        Destroy(other.gameObject);

        PlayEffect(transform.position);
    }

    void HandleArmor(Collider2D other) {
        // if armor is full return
        if (playerHealth.IsArmorFull()) {
            return;
        }

        // increase armor betwen min an max value
        float armorIncrease = Random.Range(minArmorIncrease, maxArmorIncrease);
        playerHealth.UpdateArmor(armorIncrease);
        Destroy(other.gameObject);

        PlayEffect(transform.position);
    }

    void PlayEffect(Vector3 position) {
        GameObject effect = Instantiate(itemPickupEffect, position, Quaternion.identity);
        Destroy(effect, 1f);
    }
}
