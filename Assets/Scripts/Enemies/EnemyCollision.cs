using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] GameObject hitEffect;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FireBall")
        {
            GameObject effect = Instantiate(hitEffect, transform.position + new Vector3(0f, 0f, 0f), Quaternion.identity);
            Destroy(effect, 1f);
        }
    }
}
