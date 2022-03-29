using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayer : MonoBehaviour
{
    Rigidbody2D rb2d;

    [SerializeField] GameObject player;
    [SerializeField] float moveSpeed;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        RotateSprite();

        Vector2 distance = new Vector2(player.transform.position.x - transform.position.x, 0f);
        distance = distance.normalized;

        rb2d.AddForce(distance * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
    }

    void RotateSprite()
    {
        float distance = player.transform.position.x - transform.position.x;


        Vector3 enemyLocalScale = transform.localScale;
        if (distance < 0f)
        {
            enemyLocalScale.x = (transform.localScale.x > 0 ? -1 : 1) * transform.localScale.x;
        }
        else if (distance > 0f)
        {
            enemyLocalScale.x = (transform.localScale.x > 0 ? 1 : -1) * transform.localScale.x;
        }
        transform.localScale = enemyLocalScale;
    }
}
