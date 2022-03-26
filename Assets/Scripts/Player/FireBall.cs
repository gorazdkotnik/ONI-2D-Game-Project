using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] GameObject hitEffect;

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject effect = Instantiate(hitEffect, transform.position + new Vector3(0f, 0.75f, 0f), Quaternion.identity);
        Destroy(effect, 0.2f);
        Destroy(gameObject);
    }
}
