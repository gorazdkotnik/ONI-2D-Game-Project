using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] GameObject hitEffect;

    [Header("Options")]
    [SerializeField] float hitEffectOffsetY = 1.25f;
    [SerializeField] float destroyDistance = 15f;

    GameObject player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        // if distance from player > 15, destroy
        if (Vector3.Distance(transform.position, player.transform.position) > destroyDistance) {
            DestroyFireBall();
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyFireBall();
    }

    void DestroyFireBall() {
        GameObject effect = Instantiate(hitEffect, transform.position + new Vector3(0f, transform.position.y + hitEffectOffsetY, 0f), Quaternion.identity);
        Destroy(effect, 1f);
        Destroy(gameObject);
    }
}
