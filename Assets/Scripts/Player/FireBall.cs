using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] GameObject hitEffect;

    [Header("Options")]
    [SerializeField] float hitEffectOffsetY = 1.25f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject effect = Instantiate(hitEffect, transform.position + new Vector3(0f, transform.position.y + hitEffectOffsetY, 0f), Quaternion.identity);
        Destroy(effect, 1f);
        Destroy(gameObject);
    }
}
