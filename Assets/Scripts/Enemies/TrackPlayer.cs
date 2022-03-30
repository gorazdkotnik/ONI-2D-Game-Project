using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayer : MonoBehaviour
{
    Rigidbody2D rb2d;

    [Header("Target")]
    [SerializeField] GameObject player;

    [Header("Options")]
    [SerializeField] float moveSpeed;
    [SerializeField] float maxDistanceTrack = 10f;

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

        float distanceX = player.transform.position.x - transform.position.x;

        Vector2 distance = new Vector2(distanceX, 0f);
        distance = distance.normalized;

        if (Mathf.Abs(distanceX) > maxDistanceTrack) return;

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
